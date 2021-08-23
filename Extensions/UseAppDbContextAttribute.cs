using System.Reflection;
using mhyphen.Data;
using HotChocolate.Types;
using HotChocolate.Types.Descriptors;

namespace mhyphen.Extensions
{
    public class UseAppDbContextAttribute : ObjectFieldDescriptorAttribute
    {
        public override void OnConfigure(
            IDescriptorContext context,
            IObjectFieldDescriptor descriptor,
            MemberInfo member)
        {
            descriptor.UseDbContext<AppDbContext>();
        }
    }
}
