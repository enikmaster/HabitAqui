// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

#nullable disable

using Microsoft.AspNetCore.Mvc.Rendering;

namespace HabitAqui.Areas.Identity.Pages.Account.Manage;

/// <summary>
///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
///     directly from your code. This API may change or be removed in future releases.
/// </summary>
public static class ManageNavPages
{
    public static string Index => "Index";

    public static string Email => "Email";

    public static string ChangePassword => "ChangePassword";

    public static string GestaoUtilizadores => "GestaoUtilizadores";

    public static string DownloadPersonalData => "DownloadPersonalData";

    public static string DeletePersonalData => "DeletePersonalData";

    public static string PersonalData => "PersonalData";

    public static string TwoFactorAuthentication => "TwoFactorAuthentication";

    public static string EditarDados => "EditarDados";

    public static string GestaoHabitacoes => "GestaoHabitacoes";
    public static string GestaoLocadores => "GestaoLocadores";
    public static string GestaoCategorias => "GestaoCategorias";


    public static string GestaoCategoriasNavClass(ViewContext viewContext)
    {
        return PageNavClass(viewContext, GestaoCategorias);
    }


    public static string GestaoLocadoresNavClass(ViewContext viewContext)
    {
        return PageNavClass(viewContext, GestaoLocadores);
    }

    public static string GestaoHabitacoesNavClass(ViewContext viewContext)
    {
        return PageNavClass(viewContext, GestaoHabitacoes);
    }

    public static string EditarDadosNavClass(ViewContext viewContext)
    {
        return PageNavClass(viewContext, EditarDados);
    }

    public static string IndexNavClass(ViewContext viewContext)
    {
        return PageNavClass(viewContext, Index);
    }

    public static string GestaoUtilizadoresNavClass(ViewContext viewContext)
    {
        return PageNavClass(viewContext, GestaoUtilizadores);
    }

    public static string EmailNavClass(ViewContext viewContext)
    {
        return PageNavClass(viewContext, Email);
    }

    public static string ChangePasswordNavClass(ViewContext viewContext)
    {
        return PageNavClass(viewContext, ChangePassword);
    }

    public static string DownloadPersonalDataNavClass(ViewContext viewContext)
    {
        return PageNavClass(viewContext, DownloadPersonalData);
    }

    public static string DeletePersonalDataNavClass(ViewContext viewContext)
    {
        return PageNavClass(viewContext, DeletePersonalData);
    }

    public static string PersonalDataNavClass(ViewContext viewContext)
    {
        return PageNavClass(viewContext, PersonalData);
    }

    public static string TwoFactorAuthenticationNavClass(ViewContext viewContext)
    {
        return PageNavClass(viewContext, TwoFactorAuthentication);
    }

    public static string PageNavClass(ViewContext viewContext, string page)
    {
        var activePage = viewContext.ViewData["ActivePage"] as string
                         ?? Path.GetFileNameWithoutExtension(viewContext.ActionDescriptor.DisplayName);
        return string.Equals(activePage, page, StringComparison.OrdinalIgnoreCase) ? "active" : null;
    }
}