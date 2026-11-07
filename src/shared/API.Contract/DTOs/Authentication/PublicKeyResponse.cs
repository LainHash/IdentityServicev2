using System.Text.Json.Serialization;

namespace API.Contract.DTOs.Authentication
{
    public sealed record PublicKeyResponse(
        string KeyId,
        string Algorithm,
        string PublicKeyPem);

    public sealed record JwksResponse(
        [property: JsonPropertyName("keys")] IReadOnlyList<JwkKeyDto> Keys);

    public sealed record JwkKeyDto(
        [property: JsonPropertyName("kty")] string Kty,
        [property: JsonPropertyName("use")] string Use,
        [property: JsonPropertyName("alg")] string Alg,
        [property: JsonPropertyName("kid")] string Kid,
        [property: JsonPropertyName("n")] string N,
        [property: JsonPropertyName("e")] string E);

    public sealed record OpenIdConfigurationResponse(
        [property: JsonPropertyName("issuer")] string Issuer,
        [property: JsonPropertyName("jwks_uri")] string JwksUri,
        [property: JsonPropertyName("id_token_signing_alg_values_supported")] IReadOnlyList<string> IdTokenSigningAlgValuesSupported);

    public sealed record GenerateTestTokenRequest(
        Guid? UserPublicId = null,
        string Email = "superadmin@restaurant.local",
        string[]? Roles = null);
}

