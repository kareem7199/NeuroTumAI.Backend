using NeuroTumAI.Core;
using NeuroTumAI.Core.Exceptions;
using NeuroTumAI.Core.Identity;
using NeuroTumAI.Core.Services.Contract;
using NeuroTumAI.Core.Specifications.UserDeviceTokenSpecs;

namespace NeuroTumAI.Service.Services.UserDeviceTokenService
{
	public class UserDeviceTokenService : IUserDeviceTokenService
	{
		private readonly IUnitOfWork _unitOfWork;

		public UserDeviceTokenService(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}

		public async Task AddDeviceTokenAsync(string userId, string deviceToken)
		{
			var UserDeviceTokenRepo = _unitOfWork.Repository<UserDeviceToken>();
			var deviceTokenSpec = new UserDeviceTokenSpecifications(deviceToken);

			var existingDeviceToken = await UserDeviceTokenRepo.GetWithSpecAsync(deviceTokenSpec);

			if (existingDeviceToken is not null)
			{
				existingDeviceToken.ApplicationUserId = userId;

				UserDeviceTokenRepo.Update(existingDeviceToken);
			}
			else
			{
				var newDeviceToken = new UserDeviceToken()
				{
					ApplicationUserId = userId,
					FcmToken = deviceToken
				};

				UserDeviceTokenRepo.Add(newDeviceToken);
			}

			await _unitOfWork.CompleteAsync();
		}

		public async Task RemoveDeviceTokenAsync(string userId, string deviceToken)
		{
			var UserDeviceTokenRepo = _unitOfWork.Repository<UserDeviceToken>();
			var deviceTokenSpec = new UserDeviceTokenSpecifications(deviceToken);

			var existDeviceToken = await UserDeviceTokenRepo.GetWithSpecAsync(deviceTokenSpec);

			if (existDeviceToken is null || existDeviceToken.ApplicationUserId != userId)
				throw new NotFoundException("Device token not found or does not belong to the specified user.");

			UserDeviceTokenRepo.Delete(existDeviceToken);

			await _unitOfWork.CompleteAsync();
		}
	}
}
