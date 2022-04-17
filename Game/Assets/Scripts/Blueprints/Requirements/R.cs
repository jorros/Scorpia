namespace Blueprints.Requirements
{
    public static class R
    {
        public static Requirement Or(params Requirement[] requirements)
        {
            return new OrRequirement
            {
                Requirements = requirements
            };
        }
    }
}