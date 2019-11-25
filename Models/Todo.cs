using System.ComponentModel.DataAnnotations;

public class Todo {
	[Required]
	public int Id { get; set; }
	[Required]
	public string Description { get; set; }
	public bool IsChecked { get; set; }
	public User Owner { get; set; }
}