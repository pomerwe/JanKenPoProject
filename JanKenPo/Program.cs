using System;
using System.Collections.Generic;
using System.Linq;

namespace JanKenPo
{
  class Program
  {
    static void Main(string[] args)
    {
      JanKenPoPlayer playerOne = new JanKenPoPlayer("João");
      JanKenPoPlayer playerTwo = new JanKenPoPlayer("Maria");

      JanKenPoConfiguration config = CreateMatchConfiguration();
      JanKenPoMatch match = new JanKenPoMatch(config, playerOne, playerTwo);

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
    public JanKenPoPlayer(string name)
    {
      Name = name;
    }
  }


  class JanKenPoMatch
  {
    private readonly JanKenPoConfiguration gameConfiguration;
    private readonly JanKenPoPlayer playerOne;
    private readonly JanKenPoPlayer playerTwo;

    public JanKenPoMatch(JanKenPoConfiguration gameConfiguration, JanKenPoPlayer playerOne, JanKenPoPlayer playerTwo)
    {
      this.gameConfiguration = gameConfiguration;
      this.playerOne = playerOne;
      this.playerTwo = playerTwo;
    }

    public JanKenPoFightResult GetFightResult()
    {
      JanKenPoValue playerOneValue = gameConfiguration.PickOption();
      JanKenPoValue playerTwoValue = gameConfiguration.PickOption();

      Console.WriteLine($"{playerOne.Name} joga {playerOneValue.Name}");
      Console.WriteLine($"{playerTwo.Name} joga {playerTwoValue.Name}");

      if (playerOneValue == playerTwoValue)
      {
        return new JanKenPoFightResult("O jogo deu Empate");
      }

      string result;

      if(playerOneValue.Fight(playerTwoValue))
      {
        result = $"{playerOne.Name}";
      }
      else
      {
        result = $"{playerTwo.Name}";
      }
      result += " é o vencedor!";
      return new JanKenPoFightResult(result);
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

    public bool Fight(JanKenPoValue enemy)
    {
      return beatList.Any(w => w == enemy);
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
