namespace recipes;

public class Instruction
{
    public int Index { get; set; }
    public string Text { get; set; }
    public Instruction(int index, string text)
    {
        Index = index;
        if (text == null || text == "")
        {
            throw new ArgumentException("Text field cannot be null or empty");
        }
        Text = text;
    }

    public override string ToString()
    {
        return Index + ": " + Text;
    }
}