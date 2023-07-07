using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace BankAPI.Data.BankModels;

public partial class Client
{
    public Client()
    {
        Accounts = new HashSet<Account>();
    }
    
    public int Id { get; set; }
    [MaxLength(200, ErrorMessage = "El nombre no debe de exceder los 200 caracteres")]
    public string Name { get; set; } = null!;
    //DATA ANOTATIOIN: atributos para propiedades en los modelos
    [MaxLength(40, ErrorMessage = "El numero de telefono no debe de exceder los 40 digitos")]
    public string PhoneNumber { get; set; } = null!;
    [MaxLength(50, ErrorMessage = "El email no debe de exceder los 50 caracteres")]
    [EmailAddress(ErrorMessage = "El formato de tu email es incorrecto!")]
    public string Email { get; set; }
    
    public DateTime? RegDate { get; set; }

    [JsonIgnore]
    public virtual ICollection<Account> Accounts { get; set; } = new List<Account>();
}
