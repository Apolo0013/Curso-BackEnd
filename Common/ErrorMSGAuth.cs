
namespace BackEnd.Common.ErrorMSGAuth;
public static class ErrorMSGAuth
{
    //Error: quando o email estive errado
    public const string ERROR_EMAIL_EM_USO = "ERROR_EMAIL_EM_USO";
    //Usuario nao Encontrado
    public const string ERROR_USER_NOFIND = "ERROR_USER_NOFIND";
    //Nome ja em uso
    public const string ERROR_NOME_EM_USO = "ERROR_NOME_EM_USO";
    //usada para badrequest, Deve se usada quando algo interno fude memo.
    // ex: algum funcao teve um comportamento anormal.
    public const string ERROR_INTERNO_SISTEMA = "ERROR_INTERNO_SISTEMA";
}