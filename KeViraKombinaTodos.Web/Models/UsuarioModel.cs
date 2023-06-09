﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using KeViraKombinaTodos.Core.Models;

namespace KeViraKombinaTodos.Web.Models {
	public class UsuarioModel {
        public int UsuarioID { get; set; }
        public string Nome { get; set; }
        public string CPF { get; set; }
        public string Telefone { get; set; }
        public string Email { get; set; }
        public bool? Status { get; set; }
        public int? PerfilID { get; set; }
        public string Perfil { get; set; }
        public DateTime? DataCriacao { get; set; }
        public DateTime? DataModificacao { get; set; }
        public IDictionary<int, string> ListPerfil { get; set; }
        public UsuarioModel()
        {
            this.ListPerfil = new Dictionary<int, string>();
        }
    }
}