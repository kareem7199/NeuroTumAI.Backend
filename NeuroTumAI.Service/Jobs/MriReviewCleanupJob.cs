using Microsoft.Extensions.Logging;
using NeuroTumAI.Core.Services.Contract;
using Quartz;

namespace NeuroTumAI.Service.Jobs
{
	public class MriReviewCleanupJob : IJob
	{
		private readonly IMriScanService _mriScanService;
		private readonly ILogger<MriReviewCleanupJob> _logger;

		public MriReviewCleanupJob(IMriScanService mriScanService, ILogger<MriReviewCleanupJob> logger)
		{
			_mriScanService = mriScanService;
			_logger = logger;
		}

		public async Task Execute(IJobExecutionContext context)
		{
			try
			{
				var expiredScans = await _mriScanService.GetExpiredUnreviewedScansAsync();

				_logger.LogInformation($"Found {expiredScans.Count} expired MRI scans to auto-complete.");
				
				foreach (var expiredScan in expiredScans)
				{
					try
					{
						await _mriScanService.AutoReviewAsync(expiredScan.Id);
					}
					catch (Exception ex)
					{
						_logger.LogError(ex, $"Error processing expired MRI scan {expiredScan.Id}");
					}
				}
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, $"Error processing expired MRI scans");
			}
		}
	}
}
