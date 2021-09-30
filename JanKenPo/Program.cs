using System;
using System.Collections.Generic;
using System.Linq;

namespace JanKenPo
{
  class Program
  {
    static void Main(string[] args)
    {
      List<JanKenPoPlayer> players = new List<JanKenPoPlayer>()
      {
        new JanKenPoPlayer("João"),
        new JanKenPoPlayer("Maria"),
        new JanKenPoPlayer("Estevão"),
        new JanKenPoPlayer("José"),
      };

      JanKenPoConfiguration config = CreateMatchConfiguration();
      JanKenPoMatch match = new JanKenPoMatch(config, players);

      Console.WriteLine(match.GetFightResult().GetResult());
    }

    public static JanKenPoConfiguration CreateMatchConfiguration()
    {
      JanKenPoValue paper = new JanKenPoValue("Paper");
      JanKenPoValue scissors = new JanKenPoValue("Scissors");
      JanKenPoValue rock = new JanKenPoValue("Rock");
      JanKenPoValue lizard = new JanKenPoValue("Lizard");
      JanKenPoValue spock = new JanKenPoValue("Spock");

      paper.AddToBeatList(rock, spock);
      scissors.AddToBeatList(paper, lizard);
      rock.AddToBeatList(lizard, scissors);
      lizard.AddToBeatList(spock, paper);
      spock.AddToBeatList(scissors, rock);

      List<JanKenPoValue> configuratedValues = new List<JanKenPoValue>();
      configuratedValues.Add(paper);
      configuratedValues.Add(scissors);
      configuratedValues.Add(rock);
      configuratedValues.Add(lizard);
      configuratedValues.Add(spock);

      return new JanKenPoConfiguration(configuratedValues);
    }
    
  }

  class JanKenPoPlayer
  {
    public string Name { get; set; }
    private JanKenPoValue playerOption;

    public int LoseCount { get; set; }
    public int WinCount { get; set; }
    public int DrawCount { get; set; }
    public JanKenPoPlayer(string name)
    {
      Name = name;
    }

    public void SetPlayerOption(JanKenPoValue value)
    {
      playerOption = value;
    }

    public JanKenPoValue GetPlayerOption()
    {
      return playerOption;
    }

    public void ResetCount()
    {
      LoseCount = 0;
      WinCount = 0;
      DrawCount = 0;
    }
  }


  class JanKenPoMatch
  {
    private readonly JanKenPoConfiguration gameConfiguration;
    List<JanKenPoPlayer> players;
    List<JanKenPoPlayer> alreadyFightedOthers;

    public JanKenPoMatch(JanKenPoConfiguration gameConfiguration, List<JanKenPoPlayer> players)
    {
      this.gameConfiguration = gameConfiguration;
      this.players = players;
    }
    
    private void Fight(JanKenPoPlayer firstFighter, JanKenPoPlayer secondFighter)
    {
      if(firstFighter.GetPlayerOption() == secondFighter.GetPlayerOption())
      {
        firstFighter.DrawCount++;
        secondFighter.DrawCount++;
      }
      else
      {
        if (firstFighter.GetPlayerOption().Beats(secondFighter.GetPlayerOption()))
        {
          firstFighter.WinCount++;
          secondFighter.LoseCount++;
        }
        else
        {
          firstFighter.LoseCount++;
          secondFighter.WinCount++;
        }
      }
    }

    private void SetPlayersOptions()
    {
      players.ForEach(p =>
      {
        p.SetPlayerOption(gameConfiguration.PickOption());

        Console.WriteLine($"{p.Name} joga {p.GetPlayerOption().Name}");
      });
    }

    private void DuelAllPlayers()
    {
      alreadyFightedOthers = new List<JanKenPoPlayer>();
      for (int i = 0; i < players.Count; i++)
      {
        JanKenPoPlayer currentPlayer = players[i];

        foreach (var target in players)
        {
          if (target == currentPlayer || alreadyFightedOthers.Contains(target))
          {
            continue;
          }
          Fight(currentPlayer, target);
        }
        alreadyFightedOthers.Add(currentPlayer);
      }
    }

    private JanKenPoPlayer StartMatch()
    {
      players.ForEach(p =>
      {
        p.ResetCount();
      });
      SetPlayersOptions();
      DuelAllPlayers();

      return players.FirstOrDefault(p => p.WinCount == players.Count - 1);
    }

    public JanKenPoFightResult GetFightResult()
    {
      JanKenPoPlayer winner = null;
      while(winner == null)
      {
        winner = StartMatch();

        if(winner == null)
        {
          Console.WriteLine("Ninguém venceu!");
          Console.WriteLine("====================================");
          Console.WriteLine("====================================");
          Console.WriteLine("");
          Console.WriteLine("");
        }
        
      }

      return new JanKenPoFightResult($"O vencedor é {winner.Name}");
    }
  }

  class JanKenPoConfiguration
  {
    private readonly List<JanKenPoValue> configuration;

    public JanKenPoConfiguration(List<JanKenPoValue> configuratedValues)
    {
      this.configuration = configuratedValues;
    }

    public JanKenPoValue PickOption()
    {
      Random random = new Random(DateTime.Now.Millisecond);
      int randomNumber = random.Next(0, configuration.Count);

      return configuration[randomNumber];
    }
  }


  class JanKenPoValue
  {
    public string Name { get; set; }
    private readonly List<JanKenPoValue> beatList;
    public JanKenPoValue(string name)
    {
      Name = name;
      beatList = new List<JanKenPoValue>();
    }

    public void AddToBeatList(params JanKenPoValue[] value)
    {
      beatList.AddRange(value);
    }

    public bool Beats(JanKenPoValue enemy)
    {
      return beatList.Any(b => b == enemy);
    }
  }

  class JanKenPoFightResult
  {
    private readonly string result;

    public JanKenPoFightResult(string result)
    {
      this.result = result;
    }

    public string GetResult()
    {
      return result;
    }
  }
}
