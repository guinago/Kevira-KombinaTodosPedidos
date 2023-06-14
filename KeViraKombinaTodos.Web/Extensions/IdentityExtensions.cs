using System;
using System.Security.Claims;
using System.Security.Principal;

namespace KeViraKombinaTodos.Web.Extensions {
	public static class IdentityExtensions {
		public static string GetNomeID(this IIdentity identity) {
			var claim = ((ClaimsIdentity)identity).FindFirst("NomeID");
			// Test for null to avoid issues during local testing
			return (claim != null) ? claim.Value : string.Empty;
		}
		public static int GetPerfilID(this IIdentity identity) {
			var claim = ((ClaimsIdentity)identity).FindFirst("PerfilID");
			// Test for null to avoid issues during local testing
			return (claim != null) ? Convert.ToInt32(claim.Value) : 0;
		}
		public static int GetIdUsuarioLogado(this IIdentity identity) {
			var claim = ((ClaimsIdentity)identity).FindFirst("Id");
			// Test for null to avoid issues during local testing
			return (claim != null) ? Convert.ToInt32(claim.Value) : 0;
		}
	}
}