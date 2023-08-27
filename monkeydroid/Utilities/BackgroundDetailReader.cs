
using CommandLineSwitchPipe;
using monkeydroid.Content;
using System.Diagnostics;

namespace monkeydroid.Utilities;

// This class runs on a background thread after the VisualizerPage is refresh.
// It issues --md.detail [shader] requests for each visualizer and updates the
// corresponding cached VisualizerFile object with the results.

internal class BackgroundDetailReader
{
    private static readonly int RateLimiterMs = 500;

    // thread-safe copy used to validate / query the current server and indicate success
    private readonly string serverID;
    private readonly string serverHostname;
    private readonly int serverPortNumber;
    private bool updateSuccessful = false;

    // local copy that should be thread-safe on the main thread
    private VisualizerFile currentFile = null;


    // available but currently monkey-droid fails background reads silently
    public Exception ExceptionResult { get; private set; } = null;

    public BackgroundDetailReader(Server server)
    {
        serverID = server.Id;
        serverHostname = server.Hostname;
        serverPortNumber = server.PortNumber;
    }

    // background task
    // call via MauiProgram.BeginReadVisualizerDetails
    // cancel via AbortReadVisualizerDetails
    public async Task RequestVisualizerDetailsAsync(IReadOnlyList<VisualizerFile> vizList, CancellationToken cancellationToken)
    {
        Debug.WriteLine($"Starting background reads at {DateTime.Now}");

        // abort if we don't have any viz loaded or all descriptions were already loaded
        if (vizList.Count == 0 || !vizList.Any(v => v.Description.Equals(VisualizerFile.DefaultDescription))) return;

        try
        {
            foreach (var viz in vizList)
            {
                if(viz.Description.Equals(VisualizerFile.DefaultDescription))
                {
                    Debug.WriteLine($"Background read for {viz.Name}");

                    await Task.Delay(RateLimiterMs, cancellationToken);
                    if (cancellationToken.IsCancellationRequested) return;
                    currentFile = viz;
                    
                    await RequestDetails();
                    Debug.WriteLine($"   Uses Audio:  {currentFile.UsesAudio}");
                    Debug.WriteLine($"   Description: {currentFile.Description}");

                    await MainThread.InvokeOnMainThreadAsync(UpdateCachedVisualizerFile);
                    if (!updateSuccessful) throw new Exception("Main thread update failed.");
                }
            }
        }
        catch (Exception ex)
        {
            ExceptionResult = ex;
        }

        Debug.WriteLine($"Ending background reads at {DateTime.Now}");
    }

    // runs on the main thread, copies currentFile details to the cached equivalent
    public void UpdateCachedVisualizerFile()
    {
        updateSuccessful = false;

        var server = MauiProgram.GetCurrentServer();
        if (!serverID.Equals(server.Id)) return;

        var cachedFile = server.Visualizers.FirstOrDefault(v => v.Name.Equals(currentFile.Name));
        if(cachedFile is not null)
        {
            cachedFile.Description = currentFile.Description;
            cachedFile.UsesAudio = currentFile.UsesAudio;
            MauiProgram.SaveCache();
        }

        currentFile = null;
        updateSuccessful = true;
    }

    // runs on the background thread
    private async Task RequestDetails()
    {
        if (currentFile is null) return;
        var success = await CommandLineSwitchServer.TrySendArgs(new string[] { "--md.detail", currentFile.Name }, serverHostname, serverPortNumber);
        if (!success || string.IsNullOrWhiteSpace(CommandLineSwitchServer.QueryResponse) || CommandLineSwitchServer.QueryResponse.Length < 2) throw new Exception("Visualizer detail request failed.");
        currentFile.UsesAudio = CommandLineSwitchServer.QueryResponse.First().Equals('1');
        currentFile.Description = CommandLineSwitchServer.QueryResponse.Substring(1);
    }
}
