using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Seminario.Datos.Type.ResultType;

namespace Seminario.Api.FilterResponse;

public class SeminarioResponse : Attribute, IAlwaysRunResultFilter
{
    public void OnResultExecuting(ResultExecutingContext context)
    {
        var response = context.HttpContext.Response;
        //Si la accion se ejectuo con otro status code que OK tampoco le doy tratamiento de tarnet
        if (response.StatusCode != 200)
        {
            return;
        }

        //
        //
        var objetResult = (context.Result as ObjectResult)?.Value;
        //Ups si tengo un error en el modelo voy a pasar por aca
        //Entonces no lo quiero tratar como un error de TarnetResult.
        if (objetResult is ValidationProblemDetails)
        {
            //
            var validationProblems = (ValidationProblemDetails)objetResult;
            //
            var sb = new StringBuilder();
            //
            foreach (var error in validationProblems.Errors)
            {
                //
                var errorMessage = "";
                if (error.Value.Length > 0)
                {
                    errorMessage = error.Value[0];
                }

                //
                sb.AppendLine(errorMessage);
            }

            //
            var errorResultType = new ResultType();
            errorResultType.setError(sb.ToString());
            context.Result = new OkObjectResult(errorResultType);
            //
            return;
        }

        var result = (context.Result as ObjectResult)?.Value;
        if (result != null && result.GetType() == typeof(ResultType))
        {
            return;
        }

        try
        {
            //Intentamos devolver el resultado trimeado
            var timmedResult = TrimStringProperties(result);
            result = timmedResult;
        }
        catch (Exception)
        {
            //Nunca tiro exception si no puedo trimear
        }

        //
        var resultType = new ResultType
        {
            data = result!
        };
        context.Result = new OkObjectResult(resultType);
    }

    public void OnResultExecuted(ResultExecutedContext context) { }


    /// <summary>Trim all String properties of the given object</summary>
    public static TSelf TrimStringProperties<TSelf>(TSelf input)
    {
        //
        if (input == null)
        {
            return input;
        }

        //
        var stringProperties = input.GetType().GetProperties()
            .Where(p => p.PropertyType == typeof(string) && p.CanWrite);

        foreach (var stringProperty in stringProperties)
        {
            //
            var currentValue = (string)stringProperty.GetValue(input, null);
            if (currentValue != null)
            {
                stringProperty.SetValue(input, currentValue.Trim(), null);
            }
        }

        return input;
    }
}