using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ticket_vkr.UI.Domain.Status;

public class AddSeriesRequest
{
    [Required(ErrorMessage = "Название обязательно для заполнения")]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "Название должно быть от 2 до 100 символов")]
    [RegularExpression(@"^[a-zA-Zа-яА-Я0-9\s\-\.\,]+$", ErrorMessage = "Название должно содержать только буквы, цифры и пробелы")]
    public string Title { get; set; }

    [Required(ErrorMessage = "Жанры обязательны для заполнения")]
    public List<string> Genres { get; set; }

    [Required(ErrorMessage = "Год выхода обязателен")]
    [Range(1950, 2030, ErrorMessage = "Год должен быть от 1950 до 2030")]
    public int Year { get; set; }

    [Required(ErrorMessage = "Количество сезонов обязательно")]
    [Range(1, 50, ErrorMessage = "Количество сезонов должно быть от 1 до 50")]
    public int Seasons { get; set; }

    [Required(ErrorMessage = "Продолжительность обязательна")]
    [Range(5, 180, ErrorMessage = "Продолжительность должна быть от 5 до 180 минут")]
    public int Duration { get; set; }

    [Required(ErrorMessage = "Режиссер обязателен")]
    [StringLength(50, MinimumLength = 2, ErrorMessage = "Имя режиссера должно быть от 2 до 50 символов")]
    public string Director { get; set; }

    [Required(ErrorMessage = "Актеры обязательны")]
    public List<string> Cast { get; set; }

    [Required(ErrorMessage = "Рейтинг обязателен")]
    [Range(0.1, 10.0, ErrorMessage = "Рейтинг должен быть от 0.1 до 10.0")]
    public double Rating { get; set; }

    [Required(ErrorMessage = "Страна обязательна для заполнения")]
    [StringLength(50, MinimumLength = 2, ErrorMessage = "Название страны должно быть от 2 до 50 символов")]
    public string Country { get; set; }

    [RegularExpression(@"^[\w\-\.]+\.(jpg|jpeg|png|gif|webp)$",
       ErrorMessage = "Имя файла должно быть вида: 001.jpg, image.png")]
    public string Poster { get; set; }

    [Required(ErrorMessage = "Описание обязательно для заполнения")]
    [StringLength(2000, MinimumLength = 50, ErrorMessage = "Описание должно быть от 50 до 2000 символов")]
    public string Description { get; set; }
}

public class AddSeriesResponse
{
    public bool Success { get; set; }
    public string Message { get; set; }
    public Dictionary<string, List<string>> Errors { get; set; }
    public Guid? SeriesId { get; set; }
}