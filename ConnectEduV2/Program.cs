using ConnectEduV2.Interfaces;
using ConnectEduV2.Repositories;
using ConnectEduV2.Models;
using FluentValidation.AspNetCore;
using FluentValidation;
using ConnectEduV2.Filters;
using ConnectEduV2.Validator;
using System.Reflection;
using ConnectEduV2.Hubs;


namespace ConnectEduV2
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers()?.AddFluentValidation(fv => fv.RegisterValidatorsFromAssembly(Assembly.GetExecutingAssembly()));
            // Add services to the container.
            builder.Services.AddControllersWithViews();
            builder.Services.AddSession();
         
            builder.Services.AddScoped<ConnectEduV1Context>(); // Đăng ký DbContext
            builder.Services.AddScoped(typeof(IRepository<>), typeof(RepositoryBase<>)); // Đăng ký IRepository và RepositoryBase
            builder.Services.AddScoped<ISubjectRepository, SubjectRepository>();
            builder.Services.AddScoped<ISchoolRepositoriy, SchoolRepositoriy>();
            builder.Services.AddScoped<IDepartmentRepository, DepartmentRepository>();
            builder.Services.AddScoped<ISemesterRepository, SemesterRepository>();
            builder.Services.AddScoped<IUserRepository, UserRepository>();
            builder.Services.AddScoped<IClassRepository, ClassRepository>();
            builder.Services.AddScoped<IWalletRepository, WalletRepository>();
            builder.Services.AddScoped<IDepositTransaction, DepositTransactionRepository>();
            builder.Services.AddScoped<IClassRegistrationRepository, ClassRegistrationRepository>();
            builder.Services.AddScoped<IRegistrationStatusRepository, RegistrationStatusRepository>();
            builder.Services.AddScoped<IPurchaseTransactionRepository, PurchaseTransactionRepository>();
            builder.Services.AddScoped<IRevenueSharingRepository, RevenueSharingRepository>();
            builder.Services.AddScoped<IFeedbackRepository, FeedbackRepository>();
            builder.Services.AddScoped<MentorFillter>();
            builder.Services.AddScoped<StudentFillter>();
            builder.Services.AddScoped<StudentAndMentorFillter>();
            builder.Services.AddSignalR();

            //validator
            builder.Services.AddScoped<IValidator<User>, UserValidator>();
            // Add services to the container.
            builder.Services.AddRazorPages().AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.PropertyNamingPolicy = null;
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();
            app.UseSession();
            app.MapHub<ReportHub>("/reportHub");
            app.MapHub<ChatHub>("/chatHub");
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers(); // Bạn cũng cần map controllers ở đây nếu bạn sử dụng controllers

                endpoints.MapRazorPages(); // Bảo đảm rằng Razor Pages được sử dụng

                // Đặt trang mặc định tại đây
                endpoints.MapFallbackToPage("/Home/Home");
            });



            app.Run();
        }
    }
}