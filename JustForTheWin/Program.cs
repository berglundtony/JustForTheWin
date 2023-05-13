namespace JustForTheWin
{
    public class Program
    {
        static void Main(string[] args)
        {
            PlayBalls game = new PlayBalls();
            game.Start();
        }
    }


    public class PlayBalls
    {
        public static bool ExtraPick { get; private set; }
        public static int PlayingCredits { get; private set; }
        public static int NumberOfRounds { get; private set; }

        public void Start()
        {
            PlayingCredits = 0;
            double rtp = 0;
            NumberOfRounds = 1;
            ChooseNORandPlayCredit(ref rtp);
            NumberOfRounds = 1;

            while (NumberOfRounds > 0)
            {
                Console.WriteLine();
                Console.WriteLine("The game is over");
                Console.WriteLine();
                Console.WriteLine("Do you want to play again? y/n.");

                if (Console.ReadLine() == "y")
                {
                    Console.Clear();
                    ChooseNORandPlayCredit(ref rtp);
                    NumberOfRounds = 1;
                }
                else
                {
                    Environment.Exit(0);
                }
            }
        }

        private void ChooseNORandPlayCredit(ref double rtp)
        {
            int noR;
            Console.WriteLine();

            while (NumberOfRounds > 0)
            {
                Console.WriteLine("How many rounds?");

                if (int.TryParse(Console.ReadLine(), out noR))
                {
                    NumberOfRounds = noR;
                    rtp = PlayBallsInRound(NumberOfRounds, 0);
                    PlayingCredits = Functions.RoundNum((int)rtp);

                    if (PlayingCredits != 0)
                    {
                        Console.WriteLine();
                        Console.WriteLine($"You got {PlayingCredits} EURO in return to player bonus after playing.");
                        Console.WriteLine();
                        Console.WriteLine("Do you want to play again? y/n.");

                        if (Console.ReadLine() == "y")
                        {
                            Console.WriteLine();
                            Console.Clear();

                            if (PlayingCredits > 0)
                            {
                                rtp = PlayBallsInRound(0, PlayingCredits);
                            }
                        }
                        else
                        {
                            Environment.Exit(0);
                        }
                    }
                }
            }
        }

        private static double PlayBallsInRound(int numberOfRounds, int playingCredits)
        {
            int[] wonAndPlayingCredits = new int[2];
            int ballsInTheBasket = 20;
            int wonCredits = 0;
            int costPerPick = 10;
            int winAmount = 20;
            int rtp = 0;

            string[] ballTypes = new string[]
            {
                "win","win","win","win","win",
                "extra pick",
                "no win", "no win", "no win", "no win", "no win", "no win", "no win", "no win",
                "no win", "no win", "no win", "no win", "no win", "no win"
            };

            if (playingCredits > 0)
            {
                wonAndPlayingCredits = LoopByPlayingCredits(ref wonAndPlayingCredits, ballsInTheBasket, ref wonCredits, costPerPick, winAmount, ballTypes);
            }
            else
            {
                wonAndPlayingCredits = LoopByNumberOfRounds(ref wonAndPlayingCredits, ballsInTheBasket, ref wonCredits, costPerPick, winAmount, ballTypes);
            }
            if (wonAndPlayingCredits[1] != 0)
                rtp = Functions.ReturnToPlayer(wonAndPlayingCredits[0], wonAndPlayingCredits[1]);
            else
                rtp = 0;

            return rtp;
        }
        private static int[] LoopByNumberOfRounds(ref int[] wonAndPlayingCredits, int ballsInTheBasket, ref int wonCredits, int costPerPick, int winAmount, string[] ballTypes)
        {
            ExtraPick = false;
            PlayingCredits = 0;
            Random random = new Random();
            int i = 0;
            while (i <= NumberOfRounds)
            {
                if (i == 1 && !ExtraPick)
                {
                    NumberOfRounds -= 1;
                    PlayingCredits += costPerPick;
                }

                if (NumberOfRounds > 0)
                {
                    i = 1;
                    wonAndPlayingCredits[1] = PlayingCredits;
                    Console.WriteLine($"You have: {NumberOfRounds} times to play.");
                    Console.WriteLine();
                    Console.WriteLine("Choose a ball by pressing Enter...");
                    Console.ReadLine();
                    // Draw the ball from the basket (random indexing)
                    int ballIndex = random.Next(0, ballsInTheBasket);
                    string ballType = ballTypes[ballIndex];

                    // Update the player's score based on the ball type
                    wonAndPlayingCredits[0] = UpdateScore(ref wonCredits, winAmount, ballType, winAmount, costPerPick);
                }
                else
                {
                    i = 0;
                    break;
                }
            }
            return wonAndPlayingCredits;
        }

        private static int[] LoopByPlayingCredits(ref int[] wonAndPlayingCredits, int ballsInTheBasket, ref int wonCredits, int costPerPick, int winAmount, string[] ballTypes)
        {
            Random random = new Random();
            NumberOfRounds = 0;

            int i = 1;

            while (i < PlayingCredits && PlayingCredits >= 10)
            {
                if (PlayingCredits == 10 && ExtraPick == true)
                {
                    // Draw the ball from the basket (random indexing)
                    int ballIndex2 = random.Next(0, ballsInTheBasket);
                    string ballType2 = ballTypes[ballIndex2];
                    wonAndPlayingCredits[1] = PlayingCredits;
                    Console.WriteLine();
                    Console.WriteLine($"You have: {PlayingCredits} EURO to play for.");
                    Console.WriteLine();
                    Console.WriteLine("Choose a ball by pressing Enter...");
                    Console.ReadLine();
                    wonAndPlayingCredits[0] = UpdateScore(ref wonCredits, winAmount, ballType2, ballsInTheBasket, costPerPick);
                }
                else if (PlayingCredits > 10)
                {
                    if (!ExtraPick && i != 1)
                    {
                        PlayingCredits -= costPerPick;
                    }

                    wonAndPlayingCredits[1] = PlayingCredits;
                    Console.WriteLine($"You have: {PlayingCredits} EURO to play for.");
                    Console.WriteLine();
                    Console.WriteLine("Choose a ball by pressing Enter...");
                    Console.ReadLine();

                    // Draw the ball from the basket (random indexing)
                    int ballIndex = random.Next(0, ballsInTheBasket);
                    string ballType = ballTypes[ballIndex];
                    i = 2;
                    // Update the player's score based on the ball type
                    wonAndPlayingCredits[0] = UpdateScore(ref wonCredits, winAmount, ballType, ballsInTheBasket, costPerPick);
                }
                else
                {
                    wonAndPlayingCredits[1] -= 10;
                    break;
                }
            }
            return wonAndPlayingCredits;
        }

        private static int UpdateScore(ref int wonCredits, int winAmount, string ballType, int ballsInTheBasket, int costPerPick)
        {
            if (ballType == "win")
            {
                ExtraPick = false;
                wonCredits += winAmount;
                PlayingCredits += winAmount;

                Console.WriteLine("Congratulation! You won 20 EURO");
                if (NumberOfRounds > 0)
                    Console.WriteLine($"You have {wonCredits} EURO in won credits.");
            }

            if (ballType == "extra pick")
            {
                ExtraPick = true;
                Console.WriteLine("You got an extra pic!");
            }
            if (ballType == "no win")
            {
                ExtraPick = false;
                Console.WriteLine("Sorry, you didn't won this time.");
            };
            return wonCredits;
        }

    }
}