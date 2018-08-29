using AutoMapper;
using HealthBlog.Web.Mapping;

namespace HealthBlog.Tests.Mocks
{
	public static class MockAutoMapper
	{
		static MockAutoMapper()
		{
			Mapper.Initialize(config => config.AddProfile<AutoMapperProfile>());
		}

		public static IMapper GetAutoMapper() => Mapper.Instance;
	}
}
