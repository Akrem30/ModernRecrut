using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using ModernRecrut.MVC.Areas.Identity.Data;
using ModernRecrut.MVC.Extensions;
using ModernRecrut.MVC.Data;

namespace ModernRecrut.MVC.Configuration
{
    public static class IdentityConfig
    {
        public static void AddIdentityConfiguration(this IServiceCollection services)
        {
            services.AddDefaultIdentity<Utilisateur>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddErrorDescriber<CustomIdentityErrorDescriber>()
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<IdentityContext>();
        }
    }
}
