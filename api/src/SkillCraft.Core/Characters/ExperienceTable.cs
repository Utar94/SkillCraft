namespace SkillCraft.Core.Characters
{
  internal class ExperienceTable
  {
    private static readonly ushort[] _increments = new ushort[]
    {
      100, 300, 700, 1300, 2100,
      3100, 4300, 5700, 7300, 9100,
      11100, 13300, 15700, 18300, 21100,
      24100, 27300, 30700, 34300, 38100
    };

    private readonly Dictionary<int, LevelExperience> _levelExperiences = new();

    public ExperienceTable()
    {
      LevelExperience? last = null;
      for (int level = 0; level <= MaxLevel; level++)
      {
        LevelExperience current;
        if (last == null)
        {
          current = new LevelExperience(_increments[level]);
        }
        else if (level >= _increments.Length)
        {
          current = new LevelExperience(last);
        }
        else
        {
          current = new LevelExperience(_increments[level], last);
        }

        _levelExperiences.Add(level, current);

        last = current;
      }
    }

    public static int MaxLevel => _increments.Length;

    public ushort? GetIncrement(int level)
    {
      if (_levelExperiences.TryGetValue(level, out LevelExperience? levelExperience))
      {
        return levelExperience.Increment;
      }
      else
      {
        throw new ArgumentOutOfRangeException(nameof(level));
      }
    }

    public int GetLevel(int experience)
    {
      if (experience < 0)
      {
        throw new ArgumentOutOfRangeException(nameof(experience));
      }

      for (int level = MaxLevel; level > 0; level--)
      {
        LevelExperience levelExperience = _levelExperiences[level];
        if (experience >= levelExperience.Threshold)
        {
          return level;
        }
      }

      return 0;
    }

    public int GetThreshold(int level)
    {
      if (_levelExperiences.TryGetValue(level, out LevelExperience? levelExperience))
      {
        return levelExperience.Threshold;
      }
      else
      {
        throw new ArgumentOutOfRangeException(nameof(level));
      }
    }
  }

  internal class LevelExperience
  {
    public LevelExperience(ushort increment) : this(increment, null)
    {
    }
    public LevelExperience(LevelExperience last) : this(null, last)
    {
    }
    public LevelExperience(ushort? increment, LevelExperience? last)
    {
      Increment = increment;

      if (last != null)
      {
        Threshold = last.Threshold + (last.Increment ?? 0);
      }
    }

    public ushort? Increment { get; }
    public int Threshold { get; }
  }
}
