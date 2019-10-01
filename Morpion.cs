using System;
using System.Collections.Generic;


namespace Morpion
{
    class Instance
    {
        static void Main()
        {
            Morpion Instance = new Morpion(5, 5);
            Instance.Start();
        }
    }

    
    class Morpion
    {
        private enum TOKEN { VIDE, J1, J2}
        public enum PARAMETRE { ROTATE, RANDOM, FROZEN1, FROZEN2, LOSER, WINNER, USER_PREVIEW, PC_SIM, VALIDATE }
        private readonly Random Rand = new Random();
        public int OnBoardToken { get; private set; }
        private delegate void PLAYER(TOKEN joueur);
        private readonly PLAYER player_1;
        private readonly PLAYER player_2;
        private readonly TOKEN[] Board;
        public readonly int X;
        public readonly int Y;
        private TOKEN FirstPlayer;
        public PARAMETRE START;
        public Morpion()
        {
            X = 0;
            Y = 0;
            player_1 += USER_PLAYER;
            player_2 += AI_COMPUTER;
            START = PARAMETRE.FROZEN1;
            FirstPlayer = TOKEN.J2;
            Board = new TOKEN[9];
        }
        public Morpion(int X, int Y, int NB_PLAYER = 1)
        {
            
            if (NB_PLAYER > 0) player_1 += USER_PLAYER;
            else player_1 += AI_COMPUTER;
            if (NB_PLAYER > 1) player_2 += USER_PLAYER;
            else player_2 += AI_COMPUTER;
            this.X = X;
            this.Y = Y;
            START = PARAMETRE.FROZEN1;
            FirstPlayer = TOKEN.J2;
            Board = new TOKEN[9];
        }  
        public void Start()
        {
            TOKEN player = TOKEN.J2;
            bool Victory;
            do
            {
                InitBoard();
                SelectFirstPlayer(ref player);
                do
                {
                    player = GetReverseToken(player);
                    if (player == TOKEN.J1) player_1(player);
                    else player_2(player);
                    Victory = VictoryState(player);
                } while (OnBoardToken != 9 && !Victory);

                DrawText(0, 8, Victory ? ("Joueur \"" + (player == TOKEN.J1 ? "X" : "O") + "\" a gagné!") : "Match nul !");
                DrawText(0, 9, "Rejouer? (enter)");
            } while (Console.ReadKey(true).Key == ConsoleKey.Enter);
        }
        private void AI_COMPUTER(TOKEN ai_computer)
        {
            List<int> WinMove = new List<int>();
            List<int> NulMove = new List<int>();
            List<int> LoseMove = new List<int>();
            TOKEN MovePlay;
            for (int i = 0; i < 9; i++)
                if (GetToken(i) == TOKEN.VIDE)
                {
                    MovePlay = RecursiveSimulation(ai_computer, i);
                    if (ai_computer == MovePlay) WinMove.Add(i);
                    else if (GetReverseToken(ai_computer) == MovePlay) LoseMove.Add(i);
                    else NulMove.Add(i);
                }
            if (WinMove.Count > 0) AddToken(ai_computer, WinMove[Rand.Next(WinMove.Count)]);
            else if (NulMove.Count > 0) AddToken(ai_computer, NulMove[Rand.Next(NulMove.Count)]);
            else if (LoseMove.Count > 0) AddToken(ai_computer, LoseMove[Rand.Next(LoseMove.Count)]);
        }
        private void USER_PLAYER(TOKEN User_Player)
        {
            int Play = -1;
            char Input;
            ClearText(0, 8, 20);
            DrawText(0, 8, "Joueur \"" + (User_Player == TOKEN.J1 ? "X" : "O") + "\"!");
            do
            {
                Input = Console.ReadKey(true).KeyChar;
                RemoveToken(Play);
                if (int.TryParse(Input.ToString(), out int inputPlay) && inputPlay != 0 && (Board[inputPlay - 1] == TOKEN.VIDE))
                {
                    Play = inputPlay - 1;
                    AddToken(User_Player, Play, PARAMETRE.USER_PREVIEW);  
                }
            } while (Input != (char)ConsoleKey.Enter || Play == -1);
            AddToken(User_Player, Play);
        }
        private TOKEN RecursiveSimulation(TOKEN Player, int token)
        {
            AddToken(Player, token, PARAMETRE.PC_SIM);
            if (VictoryState(Player))
            {
                RemoveToken(token, PARAMETRE.PC_SIM);
                return Player;
            }
            if (OnBoardToken == 9)
            {
                RemoveToken(token, PARAMETRE.PC_SIM);
                return TOKEN.VIDE;
            }
            TOKEN NextMove;
            bool WinMove = true;
            for (int i = 0; i < 9; i++)
                if (GetToken(i) == TOKEN.VIDE)
                {
                    NextMove = RecursiveSimulation(GetReverseToken(Player), i);
                    if (NextMove == GetReverseToken(Player))
                    {
                        RemoveToken(token, PARAMETRE.PC_SIM);
                        return GetReverseToken(Player);
                    }
                    else if (NextMove == TOKEN.VIDE) WinMove = false;
                }
                

            RemoveToken(token, PARAMETRE.PC_SIM);
            return WinMove ? Player : TOKEN.VIDE;
        }
        private bool VictoryState(TOKEN Player)
        {
            for (int i = 0; i < 3; i++)
            {
                if (GetToken(i) == Player && GetToken(i + 3) == Player && GetToken(i + 6) == Player) return true;
                if (GetToken(i * 3) == Player && GetToken(i * 3 + 1) == Player && GetToken(i * 3 + 2) == Player) return true;
            }
            if (GetToken(4) == Player)
            {
                return GetToken(0) == Player && GetToken(8) == Player || GetToken(6) == Player && GetToken(2) == Player;
            }
            else return false;
            ;
        }
        private void DrawTable()
        {
            DrawText(0, 6, " ----------- ");
            for (int j = 0; j <= 4; j += 2)
            {
                for (int i = 0; i <= 12; i += 4) DrawText(i, j + 1, "|");
                DrawText(0, j, " ----------- ");
            }
        }
        private void DrawText(int posX, int posY, string text)
        {
            posX += X;
            posY += Y;
            Console.SetCursorPosition(posX, posY);
            Console.Write(text);
            Console.SetCursorPosition(posX, posY + 1);
        }
        private void ClearText(int posX, int posY, int size)
        {
            posX += X;
            posY += Y;
            Console.SetCursorPosition(posX, posY);
            while (size-- > 0) Console.Write(' ');
        }
        private void DrawToken(string TokenLabel, int TokenPlace)
        {
            DrawText((2 - (8 - TokenPlace) % 3) * 4 + 2, (8 - TokenPlace) / 3 * 2 + 1, TokenLabel);
        }
        private void AddToken(TOKEN Player, int TokenPlace, PARAMETRE PARAM = PARAMETRE.VALIDATE)
        {
            if (TokenPlace != -1 && GetToken(TokenPlace)==TOKEN.VIDE)
            {
                OnBoardToken++;
                Board[TokenPlace] = Player;
                if (PARAM == PARAMETRE.USER_PREVIEW) DrawToken(Player == TOKEN.J1 ? "x" : "o", TokenPlace);
                if (PARAM == PARAMETRE.VALIDATE) DrawToken(Player == TOKEN.J1 ? "X" : "O", TokenPlace);
            }

        }
        private TOKEN GetToken(int TokenPlace)
        {
            return Board[TokenPlace];
        }
        private void RemoveToken(int token, PARAMETRE PARAM = PARAMETRE.USER_PREVIEW)
        {
            if (token != -1 && GetToken(token) != TOKEN.VIDE)
            {
                OnBoardToken--;
                Board[token] = TOKEN.VIDE;
                if (PARAM == PARAMETRE.USER_PREVIEW) DrawToken(" ", token);
            }
        }
        private static TOKEN GetReverseToken(TOKEN A)
        {
            return A == TOKEN.J1 ? TOKEN.J2 : TOKEN.J1;
        }
        private void InitBoard()
        {
            for (int current = 0; current < 9; current++) Board[current] = 0;
            Console.Clear();
            DrawTable();
            OnBoardToken = 0;
        }
        private void SelectFirstPlayer(ref TOKEN player)
        {
            switch (START)
            {
                case PARAMETRE.FROZEN1:
                    player = TOKEN.J2;
                    break;
                case PARAMETRE.FROZEN2:
                    player = TOKEN.J1;
                    break;
                case PARAMETRE.ROTATE:
                    FirstPlayer = GetReverseToken(FirstPlayer);
                    player = FirstPlayer;
                    break;
                case PARAMETRE.RANDOM:
                    player = Rand.Next(100) > 49 ? TOKEN.J1 : TOKEN.J2;
                    break;
                case PARAMETRE.WINNER:
                    player = GetReverseToken(player);
                    break;
                default:
                    //LOSER FIRST
                    break;
            }
        }
    }
}
