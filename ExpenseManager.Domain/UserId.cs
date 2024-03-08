namespace ExpenseManager.Domain;

public record struct UserId
{
    public int Id { get; }

    private UserId(int id) => this.Id = id;

    public static UserId FromInt(int id) => new UserId(id);
}