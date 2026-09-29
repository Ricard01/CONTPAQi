using System.Text;
using SDK.Comercial.Application.Common.Exceptions;

namespace SDK.Comercial.Infrastructure.Sdk.Native;

internal static class SdkErrores
{
    /// <summary>Lanza <see cref="ComercialSdkException"/> si el código devuelto por el SDK no es 0.</summary>
    public static void Verificar(int codigo)
    {
        if (codigo != 0)
        {
            throw new ComercialSdkException(codigo, Mensaje(codigo));
        }
    }

    public static string Mensaje(int codigo)
    {
        var mensaje = new StringBuilder(ComercialSdkNative.TamanoBuffer);
        ComercialSdkNative.fError(codigo, mensaje, mensaje.Capacity);
        return mensaje.ToString().Trim();
    }
}
