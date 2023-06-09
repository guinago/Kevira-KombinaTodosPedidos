﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;

namespace KeViraKombinaTodos.Web.Models
{
    public class IndexViewModel
    {
        public bool HasPassword { get; set; }
        public IList<UserLoginInfo> Logins { get; set; }
        public string PhoneNumber { get; set; }
        public bool TwoFactor { get; set; }
        public bool BrowserRemembered { get; set; }
    }
    public class ManageLoginsViewModel
    {
        public IList<UserLoginInfo> CurrentLogins { get; set; }
        public IList<AuthenticationDescription> OtherLogins { get; set; }
    }
    public class FactorViewModel
    {
        public string Purpose { get; set; }
    }
    public class SetPasswordViewModel
    {
        [Required, DataType(DataType.Password), Display(Name = "Nova Senha")]
        [StringLength(100, ErrorMessage = "O {0} deve ter pelo menos {2} caracteres.", MinimumLength = 6)]
        public string NewPassword { get; set; }
        [DataType(DataType.Password), Display(Name = "Confirme a nova senha")]
        [Compare("NewPassword", ErrorMessage = "A nova senha e a senha de confirmação não coincidem.")]
        public string ConfirmPassword { get; set; }
    }
    public class ChangePasswordViewModel
    {
        [Required, DataType(DataType.Password), Display(Name = "Senha Atual")]
        public string OldPassword { get; set; }
        [Required, DataType(DataType.Password), Display(Name = "Nova Senha")]
        [StringLength(100, ErrorMessage = "O {0} deve ter pelo menos {2} caracteres.", MinimumLength = 6)]
        public string NewPassword { get; set; }
        [DataType(DataType.Password), Display(Name = "Confirme a nova senha")]
        [Compare("NewPassword", ErrorMessage = "A nova senha e a senha de confirmação não coincidem.")]
        public string ConfirmPassword { get; set; }
    }
    public class AddPhoneNumberViewModel
    {
        [Required, Phone, Display(Name = "Telefone")]
        public string Number { get; set; }
    }
    public class VerifyPhoneNumberViewModel
    {
        [Required, Display(Name = "Código")]
        public string Code { get; set; }
        [Required, Phone, Display(Name = "Telefone")]
        public string PhoneNumber { get; set; }
    }
    public class ConfigureTwoFactorViewModel
    {
        public string SelectedProvider { get; set; }
        public ICollection<System.Web.Mvc.SelectListItem> Providers { get; set; }
    }
}