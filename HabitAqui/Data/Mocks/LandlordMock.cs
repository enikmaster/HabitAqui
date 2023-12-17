using HabitAqui.Models;
using MessagePack;
using System;
using System.Collections.Generic;

namespace HabitAqui.Data.Mocks
{
    public static class LandlordMock
    {
        public static List<DetalhesUtilizador> GenerateGestorPrincipalMocks()
        {
            var gestores = new List<DetalhesUtilizador>
        {
            new DetalhesUtilizador
            {
                UserName = "gestorPrincipal_remax@isec.pt",
                Email = "gestorPrincipal_remax@isec.pt",
                Nome = "Lourenco",
                Apelido = "Mcbride",
                Nif = "100000000",
                EmailConfirmed = true,
                PhoneNumberConfirmed = true,
                Localizacao = new Localizacao
                {
                    Morada = "Praceta Padre Jose",
                    CodigoPostal = "3000-123",
                    Cidade = "Coimbra",
                    Pais = "Portugal"
                },
                Active = true,
            },
            new DetalhesUtilizador
            {
                UserName = "gestorPrincipal_era@isec.pt",
                Email = "gestorPrincipal_era@isec.pt",
                Nome = "Joao",
                Apelido = "Marques",
                Nif = "200000000",
                EmailConfirmed = true,
                PhoneNumberConfirmed = true,
                Localizacao = new Localizacao
                {
                    Morada = "Rua do buraco",
                    CodigoPostal = "3000-000",
                    Cidade = "Figueira Foz",
                    Pais = "Portugal"
                },
                Active = true,
            },
            new DetalhesUtilizador
            {
                UserName = "gestorPrincipal_kw@isec.pt",
                Email = "gestorPrincipal_kw@isec.pt",
                Nome = "Filipe",
                Apelido = "Carvalho",
                Nif = "300000000",
                EmailConfirmed = true,
                PhoneNumberConfirmed = true,
                Localizacao = new Localizacao
                {
                    Morada = "Rua do fundo",
                    CodigoPostal = "3000-100",
                    Cidade = "Arouca",
                    Pais = "Portugal"
                },
                Active = true,
            }
        };

            return gestores;
        }

        public static List<DetalhesUtilizador> GenerateFuncionarioMocks()
        {
            var funcionarios = new List<DetalhesUtilizador>
        {
            new DetalhesUtilizador
            {
                UserName = "funcionariotiago_remax@isec.pt",
                Email = "funcionariotiago_remax@isec.pt" ,
                Nome = "Tiago",
                Apelido = "Gouveia",
                Nif = "100000003",
                EmailConfirmed = true,
                PhoneNumberConfirmed = true,
                Localizacao = new Localizacao
                {
                    Morada = "Praceta Padre Jose",
                    CodigoPostal = "3000-123",
                    Cidade = "Coimbra",
                    Pais = "Portugal"
                },
                Active = true,
            },
            new DetalhesUtilizador
            {
                UserName = "funcionariojoao_era@isec.pt",
                Email = "funcionariojoao_era@isec.pt",
                Nome = "Joao",
                Apelido = "Mesquita",
                Nif = "200000003",
                EmailConfirmed = true,
                PhoneNumberConfirmed = true,
                Localizacao = new Localizacao
                {
                    Morada = "Rua do buraco",
                    CodigoPostal = "3000-000",
                    Cidade = "Figueira Foz",
                    Pais = "Portugal"
                },
                Active = true,
            },
            new DetalhesUtilizador
            {
                UserName = "funcionariofilipe_kw@isec.pt",
                Email = "funcionariofilipe_kw@isec.pt",
                Nome = "Filipe",
                Apelido = "Cusco",
                Nif = "300000003",
                EmailConfirmed = true,
                PhoneNumberConfirmed = true,
                Localizacao = new Localizacao
                {
                    Morada = "Rua do fundo",
                    CodigoPostal = "3000-100",
                    Cidade = "Arouca",
                    Pais = "Portugal"
                },
                Active = true,
            },
            new DetalhesUtilizador
            {
                UserName = "lisandro@isec.pt",
                Email = "lisandro@isec.pt",
                Nome = "Lisandro",
                Apelido = "Menezes",
                Nif = "123456789",
                EmailConfirmed = true,
                PhoneNumberConfirmed = true,
                Localizacao = new Localizacao
                {
                    Morada = "Rua Do Tacho",
                    CodigoPostal = "1234-123",
                    Cidade = "Guimarães",
                    Pais = "Portugal"
                },
                Active = true,
            },
             
            new DetalhesUtilizador {
                UserName = "alexandre@isec.pt",
                Email = "alexandre@isec.pt",
                Nome = "Alexandre",
                Apelido = "Caravela",
                Nif = "987654321",
                EmailConfirmed = true,
                PhoneNumberConfirmed = true,
                Localizacao = new Localizacao
                {
                    Morada = "Rua Do Giro",
                    CodigoPostal = "2393-123",
                    Cidade = "Faro",
                    Pais = "Portugal"
                },
                Active = true
            },
            new DetalhesUtilizador {
                UserName = "filipe@isec.pt",
                Email = "filipe@isec.pt",
                Nome = "Filipe",
                Apelido = "Trigo",
                Nif = "922222189",
                EmailConfirmed = true,
                PhoneNumberConfirmed = true,
                Localizacao = new Localizacao
                {
                    Morada = "Rua Do Passeio",
                    CodigoPostal = "4000-123",
                    Cidade = "Lisboa",
                    Pais = "Portugal"
                },
                Active = true
            }
        };

            return funcionarios;
        }


        public static List<Locador> GenerateLandlordMocks(List<DetalhesUtilizador> gestores, List<DetalhesUtilizador> funcionarios)
        {
            var locadores = new List<Locador>
        {
            new Locador
            {
                UserName = "remax@isec.pt",
                Email = "remax@isec.pt",
                Nome = "remax",
                Apelido = "lda",
                Nif = "100000001",
                EmailConfirmed = true,
                PhoneNumberConfirmed = true,
                Localizacao = new Localizacao
                {
                    Morada = "Praceta Padre Jose",
                    CodigoPostal = "3000-123",
                    Cidade = "Coimbra",
                    Pais = "Portugal"
                },
                Active = true,
                DataInicioSubscricao = DateTime.Now,
                EstadoDaSubscricao = "Active",
                Administradores = new List<DetalhesUtilizador> { gestores[0], funcionarios[0]},
                
            },
            new Locador
            {
                UserName = "era@isec.pt",
                Email = "era@isec.pt",
                Nome = "era",
                Apelido = "lda",
                Nif = "200000001",
                EmailConfirmed = true,
                PhoneNumberConfirmed = true,
                Localizacao = new Localizacao
                {
                    Morada = "Rua do buraco",
                    CodigoPostal = "3000-000",
                    Cidade = "Figueira Foz",
                    Pais = "Portugal"
                },
                Active = true,
                DataInicioSubscricao = DateTime.Now,
                EstadoDaSubscricao = "Active",
                Administradores = new List<DetalhesUtilizador> { gestores[1], funcionarios[1] },
            },
            new Locador
            {
                UserName = "kw@isec.pt",
                Email = "kw@isec.pt",
                Nome = "kw",
                Apelido = "lda",
                Nif = "300000001",
                EmailConfirmed = true,
                PhoneNumberConfirmed = true,
                Localizacao = new Localizacao
                {
                    Morada = "Rua do fundo",
                    CodigoPostal = "3000-100",
                    Cidade = "Arouca",
                    Pais = "Portugal"
                },
                Active = true,
                DataInicioSubscricao = DateTime.Now,
                EstadoDaSubscricao = "Active",
                Administradores = new List<DetalhesUtilizador> { gestores[2], funcionarios[2] },
            }
        };

            return locadores;
        }
    }

}
