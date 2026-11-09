using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.ValueGeneration;
using NanoidDotNet;

namespace API.Domain.Common.Helpers
{
    public static class CodeGenerator
    {
        public const string DefaultAlphabet = "23456789ABCDEFGHJKLMNPQRSTUVWXYZ";
        public const int DefaultLength = 8;

        public static string Generate(int size = DefaultLength)
        {
            return Nanoid.Generate(DefaultAlphabet, size);
        }
    }

    public class CodeValueGenerator : ValueGenerator<string>
    {
        public override bool GeneratesTemporaryValues => false;

        public override string Next(EntityEntry entry)
        {
            return CodeGenerator.Generate();
        }
    }
}

