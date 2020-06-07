namespace ImmediateReflection
{
    internal static class PublicKey
    {
        public const string Key =
#if DEPLOY_BUILD
            ", PublicKey=0024000004800000940000000602000000240000525341310004000001000100d9e0d07dfc4094926edfbffcdd039f014c8a75807633ee2c38987496d29879ea7abe134d365297ce9b2cebc853502fea34c803f753eec80cea0290670aada63070feb9e094583a83c57604613f2d1142fa51011f4727c95ace3ad7f386e19a0baa2f3789187589f7dce62d3bbd7a568640164eb39b2cfee8f50ef007cbeb21c7";
#else
            "";
#endif
    }
}