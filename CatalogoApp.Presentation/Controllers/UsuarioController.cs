using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Text.Json;
using System.Collections.Generic;
using System.Linq;
using CatalogoApp.Domain.Models;

namespace CatalogoApp.Presentation.Controllers
{
    public class UsuarioController : Controller
    {

        private readonly string _rutaUsuariosJson = Path.Combine(Directory.GetCurrentDirectory(), "data", "Usuarios.json");
        private List<Usuario> LeerUsuarios()
        {
            if (!System.IO.File.Exists(_rutaUsuariosJson))
            {

                return new List<Usuario>();
            }
            var json = System.IO.File.ReadAllText(_rutaUsuariosJson);
            return JsonSerializer.Deserialize<List<Usuario>>(json) ?? new List<Usuario>();
        }

        private void GuardarUsuarios(List<Usuario> usuarios)
        {
            var opciones = new JsonSerializerOptions { WriteIndented = true };
            var json = JsonSerializer.Serialize(usuarios, opciones);
            System.IO.File.WriteAllText(_rutaUsuariosJson, json);
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(string username, string password)
        {
            var usuarios = LeerUsuarios();


            var usuarioValido = usuarios.FirstOrDefault(u =>
                u.NombreUsuario.ToLower() == username.ToLower() &&
                u.Contrasena == password);

            if (usuarioValido != null)
            {

                return RedirectToAction("Index", "Catalogo");
            }

            ViewBag.Error = "Usuario o contraseña incorrectos. ¿Gastaste todos tus continues?";
            return View();
        }


        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(Usuario nuevoUsuario, string confirmarContrasena)
        {
            if (nuevoUsuario.Contrasena != confirmarContrasena)
            {
                ViewBag.Error = "Las contraseñas no coinciden. ¡Intenta de nuevo!";
                return View(nuevoUsuario);
            }

            var usuarios = LeerUsuarios();


            if (usuarios.Any(u => u.NombreUsuario.ToLower() == nuevoUsuario.NombreUsuario.ToLower()))
            {
                ViewBag.Error = "Ese Nickname ya está en uso. ¡Elige otro!";
                return View(nuevoUsuario);
            }


            nuevoUsuario.Id = usuarios.Any() ? usuarios.Max(u => u.Id) + 1 : 1;


            usuarios.Add(nuevoUsuario);
            GuardarUsuarios(usuarios);

            TempData["MensajeExito"] = "¡Cuenta gamer creada con éxito! Ahora puedes iniciar sesión.";
            return RedirectToAction("Login");
        }
    }
}