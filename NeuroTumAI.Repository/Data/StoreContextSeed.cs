using Microsoft.AspNetCore.Identity;
using NeuroTumAI.Core.Entities;
using NeuroTumAI.Core.Entities.Admin;
using NeuroTumAI.Core.Entities.Appointment;
using NeuroTumAI.Core.Entities.Clinic_Aggregate;
using NeuroTumAI.Core.Entities.Contact_Us;
using NeuroTumAI.Core.Identity;

namespace NeuroTumAI.Repository.Data
{
	public static class StoreContextSeed
	{

		public class CairoAddress
		{
			public string Address { get; set; }
			public double Lat { get; set; }
			public double Lng { get; set; }

			public NetTopologySuite.Geometries.Point ToPoint()
			{
				return new NetTopologySuite.Geometries.Point(Lng, Lat) { SRID = 4326 };
			}
		}

		private static string[] patientImages = { "https://kareem.blob.core.windows.net/profilepictures/patient1.jpeg" , "https://kareem.blob.core.windows.net/profilepictures/patient2.jpeg", "https://kareem.blob.core.windows.net/profilepictures/patient3.jpeg" , "https://kareem.blob.core.windows.net/profilepictures/patient4.jpeg" , "https://kareem.blob.core.windows.net/profilepictures/patient5.jpeg" };
		private static string[] doctorImages = { "https://kareem.blob.core.windows.net/profilepictures/images.jpeg", "https://kareem.blob.core.windows.net/profilepictures/images (1).jpeg" , "https://kareem.blob.core.windows.net/profilepictures/download.jpeg" , "https://kareem.blob.core.windows.net/profilepictures/download (2).jpeg" , "https://kareem.blob.core.windows.net/profilepictures/download (1).jpeg" };
		private static string[] roles = { "Patient", "Doctor" };
		private static string[] reviews = { "Pretty sure he got his degree from YouTube tutorials.", "At least he didn’t kill me... small wins, I guess.", "Not bad, not great. Like microwave food – gets the job done, barely.", "Pretty decent! He knew what he was doing… most of the time.", "Best doctor ever." };
		private static string[] times =
		{
			"09:00:00",
			"10:00:00",
			"11:00:00",
			"12:00:00",
			"13:00:00",
			"14:00:00",
			"15:00:00",
			"16:00:00",
			"17:00:00",
			"18:00:00"
		};

		private static readonly string[] names = {
	"Fawzy", "Ahmed", "Youssef", "Kareem", "Mohsen", "Omar", "Mostafa", "Ali", "Ibrahim", "Mahmoud",
	"Hassan", "Hussein", "Tarek", "Samir", "Sami", "Ayman", "Walid", "Ehab", "Nader", "Ramy",
	"Amr", "Sherif", "Nashaat", "Ashraf", "Adel", "Bassem", "Basel", "Yasser", "Gamal", "Fathy",
	"Farid", "Magdy", "Salah", "Reda", "Ziad", "Ihab", "Marwan", "Hesham", "Wael", "Anas",
	"Alaa", "Tamer", "Shady", "Essam", "Mohamed", "Khaled", "Saad", "Lotfy", "Refaat", "Nabil",
	"Sherbiny", "Arafa", "Galal", "Younes", "Hany", "Osama", "Mounir", "Nagy", "Emad", "Tawfik",
	"Fady", "Yehia", "Sameh", "Mamdouh", "Hatem", "Gaber", "Saber", "Mokhtar", "Zaki", "Maged",
	"Sayyed", "Hossam", "Hamdy", "Ezzat", "Fekry", "Shokry", "Hegazy", "Ragab", "Shaker", "Abdelrahman",
	"ElSayed", "Tolba", "Azmy", "Abdelaziz", "Hassanain", "Izzat", "Shaaban", "Barakat", "Fouad", "Soliman",
	"Younan", "Saif", "Feras", "Murad", "Yassin", "Ismail", "Bakr", "Saeed", "Yahya", "Tawheed"
};

		private static readonly CairoAddress[] CairoAddresses =
{
	new CairoAddress { Address = "شارع التحرير، الدقي", Lat = 30.037, Lng = 31.211 },
	new CairoAddress { Address = "شارع 9، المعادي", Lat = 29.960, Lng = 31.261 },
	new CairoAddress { Address = "شارع عباس العقاد، مدينة نصر", Lat = 30.068, Lng = 31.328 },
	new CairoAddress { Address = "شارع الثورة، مصر الجديدة", Lat = 30.086, Lng = 31.346 },
	new CairoAddress { Address = "شارع فيصل، الجيزة", Lat = 30.019, Lng = 31.178 },
	new CairoAddress { Address = "شارع مصطفى النحاس، مدينة نصر", Lat = 30.064, Lng = 31.345 },
	new CairoAddress { Address = "شارع قصر العيني، وسط البلد", Lat = 30.041, Lng = 31.235 },
	new CairoAddress { Address = "شارع الأزهر، الحسين", Lat = 30.045, Lng = 31.262 },
	new CairoAddress { Address = "شارع النزهة، مصر الجديدة", Lat = 30.104, Lng = 31.344 },
	new CairoAddress { Address = "شارع الهرم، الجيزة", Lat = 29.993, Lng = 31.165 },
	new CairoAddress { Address = "شارع اللبيني، فيصل", Lat = 29.988, Lng = 31.161 },
	new CairoAddress { Address = "شارع التسعين الجنوبي، التجمع", Lat = 30.002, Lng = 31.440 },
	new CairoAddress { Address = "شارع الكوربة، مصر الجديدة", Lat = 30.091, Lng = 31.330 },
	new CairoAddress { Address = "شارع النيل، العجوزة", Lat = 30.059, Lng = 31.216 },
	new CairoAddress { Address = "شارع محمد فريد، وسط البلد", Lat = 30.045, Lng = 31.242 },
	new CairoAddress { Address = "شارع المعز، الحسين", Lat = 30.049, Lng = 31.262 },
	new CairoAddress { Address = "شارع التحرير، باب اللوق", Lat = 30.044, Lng = 31.239 },
	new CairoAddress { Address = "شارع دمشق، مصر الجديدة", Lat = 30.086, Lng = 31.336 },
	new CairoAddress { Address = "شارع أحمد فخري، مدينة نصر", Lat = 30.068, Lng = 31.337 },
	new CairoAddress { Address = "شارع الطيران، مدينة نصر", Lat = 30.070, Lng = 31.313 },
	new CairoAddress { Address = "شارع حسن المأمون، مدينة نصر", Lat = 30.066, Lng = 31.356 },
	new CairoAddress { Address = "شارع البحر الأعظم، الجيزة", Lat = 30.003, Lng = 31.215 },
	new CairoAddress { Address = "شارع حسن محمد، فيصل", Lat = 30.012, Lng = 31.178 },
	new CairoAddress { Address = "شارع العروبة، مصر الجديدة", Lat = 30.093, Lng = 31.349 },
	new CairoAddress { Address = "شارع نادي الصيد، الدقي", Lat = 30.038, Lng = 31.212 },
	new CairoAddress { Address = "شارع الحجاز، مصر الجديدة", Lat = 30.099, Lng = 31.337 },
	new CairoAddress { Address = "شارع أحمد عرابي، المهندسين", Lat = 30.060, Lng = 31.210 },
	new CairoAddress { Address = "شارع جامعة الدول العربية، المهندسين", Lat = 30.059, Lng = 31.202 },
	new CairoAddress { Address = "شارع شهاب، المهندسين", Lat = 30.057, Lng = 31.202 },
	new CairoAddress { Address = "شارع مكرم عبيد، مدينة نصر", Lat = 30.065, Lng = 31.343 }
};

		private static readonly Random _random = new();

		public static CairoAddress GetRandomCairoAddress()
		{
			return CairoAddresses[_random.Next(CairoAddresses.Length)];
		}

		private static string GenerateFullName()
		{
			string firstName = names[_random.Next(names.Length)];
			string lastName = names[_random.Next(names.Length)];
			return $"{firstName} {lastName}";
		}

		public async static Task SeedAsync(StoreContext _dbContext, RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager)
		{
			if (_dbContext.UserRoles.Any()) return;
			await SeedRolesAsync(roleManager);
			await SeedPatientsAsync(userManager, _dbContext);
			await SeedDoctorsAsync(userManager, _dbContext);
			await _dbContext.SaveChangesAsync();
			await SeedContactUsMessagesAsync(_dbContext);
			await SeedAppointmentsWithReviewsAsync(_dbContext);
			await SeedAdminsAsync(_dbContext);
			await _dbContext.SaveChangesAsync();
		}
		private static async Task SeedRolesAsync(RoleManager<IdentityRole> roleManager)
		{
			foreach (var roleName in roles)
			{
				if (!await roleManager.RoleExistsAsync(roleName))
				{
					await roleManager.CreateAsync(new IdentityRole(roleName));
				}
			}
		}
		private static async Task SeedPatientsAsync(UserManager<ApplicationUser> userManager, StoreContext _dbContext)
		{
			for (int i = 0; i < 10; i++)
			{
				var newAccount = new ApplicationUser()
				{
					ProfilePicture = patientImages[_random.Next(patientImages.Length)],
					FullName = GenerateFullName(),
					Email = $"patient{i + 1}@gmail.com",
					UserName = $"Patient{i + 1}",
					Gender = Gender.Male,
					DateOfBirth = DateTime.Parse("2003-04-21"),
					EmailConfirmed = true
				};
				await userManager.CreateAsync(newAccount, "Pa$$w0rd");
				await userManager.AddToRoleAsync(newAccount, "Patient");
				var newPatient = new Patient()
				{
					ApplicationUserId = newAccount.Id,
				};
				await _dbContext.AddAsync(newPatient);
			}
		}
		private static async Task SeedDoctorsAsync(UserManager<ApplicationUser> userManager, StoreContext _dbContext)
		{
			for (int i = 0; i < 100; i++)
			{
				var newAccount = new ApplicationUser()
				{
					ProfilePicture = doctorImages[_random.Next(doctorImages.Length)],
					FullName = GenerateFullName(),
					Email = $"doctor{i + 1}@gmail.com",
					UserName = $"Doctor{i + 1}",
					Gender = Gender.Male,
					DateOfBirth = DateTime.Parse("2003-04-21"),
					EmailConfirmed = true
				};
				await userManager.CreateAsync(newAccount, "Pa$$w0rd");
				await userManager.AddToRoleAsync(newAccount, "Doctor");
				var newDoctor = new Doctor()
				{
					ApplicationUserId = newAccount.Id,
					LicenseDocumentBack = "https://kareem.blob.core.windows.net/doctor-licenses/FxtVyt2XgAA1zis.jpeg",
					LicenseDocumentFront = "https://kareem.blob.core.windows.net/doctor-licenses/FxtVyt2XgAA1zis.jpeg",
					IsApproved = i < 50
				};

				var cairoAddress = GetRandomCairoAddress();

				var location = cairoAddress.ToPoint();

				var newClinic = new Clinic()
				{
					Address = cairoAddress.Address,
					Location = location,
					PhoneNumber = "+20 100 123 4567",
					LicenseDocument = "https://kareem.blob.core.windows.net/clinic-licenses/133792076_3601250089930660_5112076721351997915_n.jpg",
					IsApproved = i < 50
				};

				for (int day = 0; day < 7; ++day)
				{
					foreach (var time in times)
					{
						var slot = new Slot()
						{
							StartTime = TimeOnly.Parse(time),
							DayOfWeek = (DayOfWeek)day
						};
						newClinic.Slots.Add(slot);
					}
				}

				newDoctor.Clinics.Add(newClinic);

				if (i < 50)
				{
					var secondClinic = new Clinic()
					{
						Address = "123 Health Street, Cairo, Egypt",
						Location = location,
						PhoneNumber = "+20 100 123 4567",
						LicenseDocument = "https://kareem.blob.core.windows.net/clinic-licenses/133792076_3601250089930660_5112076721351997915_n.jpg",
						IsApproved = false
					};
					newDoctor.Clinics.Add(secondClinic);
				}

				await _dbContext.AddAsync(newDoctor);
			}
		}
		private static async Task SeedAppointmentsWithReviewsAsync(StoreContext _dbContext)
		{
			for (int patientId = 1; patientId <= 10; patientId++)
			{
				for (int doctorId = 1; doctorId <= 50; doctorId++)
				{
					var newAppointment = new Appointment()
					{
						DoctorId = doctorId,
						PatientId = patientId,
						ClinicId = doctorId,
						Date = DateOnly.Parse("2025-04-25"),
						StartTime = TimeOnly.Parse(times[patientId - 1]),
						Status = AppointmentStatus.Completed
					};
					await _dbContext.AddAsync(newAppointment);

					int stars = Random.Shared.Next(1, 6);
					var newReview = new Review()
					{
						DoctorId = doctorId,
						PatientId = patientId,
						Stars = stars,
						Comment = reviews[stars - 1]
					};

					await _dbContext.AddAsync(newReview);
				}

			}
		}
		private static async Task SeedAdminsAsync(StoreContext _dbContext)
		{
			for (int i = 1; i <= 10; i++)
			{
				var admin = new Admin
				{
					Username = $"Admin{i}",
					Email = $"admin{i}@gmail.com",
					PasswordHash = BCrypt.Net.BCrypt.HashPassword("Pa$$w0rd")
				};

				await _dbContext.AddAsync(admin);
			}
		}
		private static async Task SeedContactUsMessagesAsync(StoreContext _dbContext)
		{
			for (int i = 1; i <= 10; i++)
			{
				var message = new ContactUsMessage
				{
					PatientId = i,
					Message = "Hello World"
				};

				await _dbContext.AddAsync(message);
			}
		}
	}
}
