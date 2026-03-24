using Microsoft.EntityFrameworkCore;
using Ticket_vkr.UI.Entities;

namespace Ticket_vkr.Infrastructure;

public class ApplicationDbContext : DbContext
{
    public DbSet<UserSeriesStatus> UserSeriesStatuses { get; set; } = null!;
    public DbSet<News> News { get; set; } = null!;
    public DbSet<Series> Series { get; set; } = null!;
    public DbSet<SeriesGenre> SeriesGenres { get; set; } = null!;
    public DbSet<SeriesActor> SeriesActors { get; set; } = null!;
    public DbSet<SeriesDirector> SeriesDirectors { get; set; } = null!;
    public DbSet<Genres> Genres { get; set; } = null!;
    public DbSet<Actor> Actors { get; set; } = null!;
    public DbSet<Director> Directors { get; set; } = null!;
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {

    }
}