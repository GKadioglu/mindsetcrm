using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UserService.Application.Constants
{
    public static class ErrorMessages
    {
        public const string EmailAlreadyExists = "Bu e-posta ile kayıtlı bir kullanıcı zaten var.";
        public const string InvalidCredentials = "Geçersiz kullanıcı adı veya şifre.";
        public const string UserNotFound = "Kullanıcı bulunamadı.";
    }
}