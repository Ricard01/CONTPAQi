using System.Text;
using SDK.Comercial.Application.Common.Exceptions;
using SDK.Comercial.Infrastructure.Sdk.Native;

namespace SDK.Comercial.Infrastructure.Sdk;

/// <summary>
/// Traduce los códigos enteros devueltos por MGWServicios.dll a excepciones comprensibles
/// para el resto de la aplicación. Vive fuera de Native porque agrega una política de adaptación:
/// un código diferente de cero se convierte en <see cref="ComercialSdkException"/>.
/// </summary>
internal static class SdkResultado
{
    /// <summary>No hace nada para cero; para cualquier otro código obtiene su mensaje y lanza una excepción.</summary>
    public static void Verificar(int codigo)
    {
        if (codigo != 0)
        {
            throw new ComercialSdkException(codigo, ObtenerMensaje(codigo));
        }
    }

    private static string ObtenerMensaje(int codigo)
    {
        var mensaje = new StringBuilder(MgwServicios.TamanoBuffer);
        MgwServicios.fError(codigo, mensaje, mensaje.Capacity);
        return mensaje.ToString().Trim();
    }
}
