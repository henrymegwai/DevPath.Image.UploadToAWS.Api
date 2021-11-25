using Amazon.Runtime;
using Amazon.S3; 
using Infrastructure.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection; 

namespace Infrastructure.DIExtensions
{
    public static class AWSS3Configuration
    {
        public static void AddAWSS3(this IServiceCollection services,
          IConfiguration Configuration)
        {
            var awsOption = Configuration.GetAWSOptions();
            awsOption.Credentials = new BasicAWSCredentials(
                Configuration.GetValue<string>("AWSS3Settings:AccessKeyId"), 
                Configuration.GetValue<string>("AWSS3Settings:SecretAccessKey")
             );

            services.AddDefaultAWSOptions(awsOption);
            services.AddAWSService<IAmazonS3>();
            services.AddScoped<IAWSS3Service, AWSS3Service>();
            services.Configure<AWSS3Settings>(Configuration.GetSection(nameof(AWSS3Settings)));
        }
    }
}
