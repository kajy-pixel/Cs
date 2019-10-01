using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;//Bibliothèques C


namespace MasterMind
{
    class Program
    {
        //=========================================BIBLIOTHEQUE C============================================//
        //Source : https://stackoverflow.com/users/8498455/sven7   "Is there a way to make fixed console?"

        const int MF_BYCOMMAND = 0x00000000;
        const int SC_MINIMIZE = 0xF020;
        const int SC_MAXIMIZE = 0xF030;
        const int SC_SIZE = 0xF000;

        [DllImport("user32.dll")]
        public static extern int DeleteMenu(IntPtr hMenu, int nPosition, int wFlags);

        [DllImport("user32.dll")]
        private static extern IntPtr GetSystemMenu(IntPtr hWnd, bool bRevert);

        [DllImport("kernel32.dll", ExactSpelling = true)]
        private static extern IntPtr GetConsoleWindow();

        static void Main(string[] args)
        {
            //============================PASSAGE DE PARAMETRES=============================//
            MasterMind.PARAM joueur1 = MasterMind.PARAM.PLAYER;
            MasterMind.PARAM joueur2 = MasterMind.PARAM.PC;
            int NBTOKENS = 5;
            if (args.Length > 0)
            {
                if (args[0].Equals("/?"))
                {
                    Console.Write("====MasterMind====\n" +
                        "MasterMind.exe <Joueur1> <Joueur2> <NB jetons>\n" +
                        "Joueur = \"PC\" \"PLAYER\"\n" +
                        "/!\\ L'IA en J2 ne tient que 6 jetons. ENJOY\n" +
                        "==================");
                    Environment.Exit(0);
                }
                try
                {
                    joueur1 = (MasterMind.PARAM)Enum.Parse(typeof(MasterMind.PARAM), args[0]);
                    if (args.Length > 1)
                        joueur2 = (MasterMind.PARAM)Enum.Parse(typeof(MasterMind.PARAM), args[1]);
                    if (args.Length > 2)
                        NBTOKENS = int.Parse(args[2]);
                }
                catch
                {
                    Console.WriteLine("Arguments non valides, pour plus d'info: \"MasterMind /?\"");
                    Environment.Exit(0);
                }
                if (NBTOKENS > 6 || NBTOKENS < 3) throw new Exception("Nombre de tokens invalide.");
            }
            //============================"INTERFACE GRAPHIQUE"=============================//
            Console.Clear();
            Console.BackgroundColor = ConsoleColor.DarkGray;
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.SetWindowSize(51, 36);
            DeleteMenu(GetSystemMenu(GetConsoleWindow(), false), SC_MINIMIZE, MF_BYCOMMAND);      /////////////////
            DeleteMenu(GetSystemMenu(GetConsoleWindow(), false), SC_MAXIMIZE, MF_BYCOMMAND);      //https://stackoverflow.com/users/8498455/sven7   "Is there a way to make fixed console?"
            DeleteMenu(GetSystemMenu(GetConsoleWindow(), false), SC_SIZE, MF_BYCOMMAND);          /////////////////

            (" ......---....---..............................           .\n" +                          /////////////////
            "`-----------------------------------------------`          .\n" +                          //https://www.text-image.com/convert/pic2ascii.cgi
            "`.-. `///////////////////////////////////////.`.-.`        .\n" +                          /////////////////
            "`.-.`:ooosooosooosooosooosooosooosooosooosooo:`.-.`        .\n" +
            "`.-.`:osooosooosooosooosooosooosooosooosoooso:`.-.`        .\n" +
            "`.-.`:ossoo                             oosso:`.-.`        .\n" +
            "`.-.`:oosso                             ossoo:`.-.`        .\n" +
            "`.-.`:oosso                             ossoo:`.-.`        .\n" +
            "`.-.`:oosoo                             osooo:`.-.`        .\n" +
            "`.-.`:ossoo                             oosso:`.-.`        .\n" +
            "`.-.`:ossoo                             oosso:`.-.`        .\n" +
            "`.-.`:oosoo                             oosoo:`.-.`        .\n" +
            "`.-.`:oosso                             ossoo:`.-.`        .\n" +
            "`.-.`:oosso                             ossoo:`.-.`        .\n" +
            "`.-.`:sssoo                             oosss:`.-.`        .\n" +
            "`.-.`:oosoo                             oosoo:`.-.`        .\n" +
            "`.-.`:ooosooosooosooosooosooosooosooosooosooo:`.-.`        .\n" +
            "`.-..........................................`..-.`        .\n" +
            "`.-.-Nintendo.Game.Boy..........................-.`        .\n" +
            "`.-.............................................-.`        .\n" +
            "`.-.............................................-.`        .\n" +
            "`.-.....................................-----...-.`        .\n" +
            "`.-...-/-    -........................--y   y-..-.`        .\n" +
            "`.-.-/---    ---...............--------l     l-.-.`        .\n" +
            "`.-..            .............--y   y---y   y.-.-.`        .\n" +
            "`.-..            -............-l     l-------B-.-.`        .\n" +
            "`.-...---    ---/-.............-y   y.-A-.......-.`        .\n" +
            "`.-.....-    -/-................-------.........-.`        .\n" +
            "`.-.......................................-.....-.`        .\n" +
            "`.-.................//l::...//a::.......----//-.-.`        .\n" +
            "`.-...............//e::...//t::.....---//+-+-+//-.`        .\n" +
            "`.-.............//s::-.://s::.....---+-+//-+-+-//.`        .\n" +
            "`.-...............................--//-+-+-//+-//          .\n" +
            "`.-..................................-//+-+-////           .\n" +
            "`.-....................................-/////              .\n" +
            "    '''''''''''':::::::::::::::::''''''                    .").Draw();
            Console.ForegroundColor = ConsoleColor.Black;
            "HHHH".Draw(9, 22);
            "HHHH".Draw(9, 23);
            "HHHHHHHHHHHH".Draw(5, 24);
            "HHHHHHHHHHHH".Draw(5, 25);
            "HHHH".Draw(9, 26);
            "HHHH".Draw(9, 27);
            Console.ForegroundColor = ConsoleColor.DarkRed;
            "mmm".Draw(41, 22);
            "mm@mm".Draw(40, 23);
            "mmm".Draw(41, 24);
            "mmm".Draw(33, 24);
            "mm@mm".Draw(32, 25);
            "mmm".Draw(33, 26);
            Console.BackgroundColor = ConsoleColor.DarkYellow;
            Console.ForegroundColor = ConsoleColor.Black;
            "|====== MASTERMIND =======|".Draw(12, 5);
            "|                         |".Draw(12, 6);
            "|                         |".Draw(12, 7);
            "|                         |".Draw(12, 8);
            "|                         |".Draw(12, 9);
            "|                         |".Draw(12, 10);
            "|                         |".Draw(12, 11);
            "|                         |".Draw(12, 12);
            "|                         |".Draw(12, 13);
            "|                         |".Draw(12, 14);
            "|_________________________|".Draw(12, 15);

            //============================BOUCLE DU MAIN=====================================//
            MasterMind instance = new MasterMind(joueur1, joueur2, NBTOKENS, 13, 6);
            do
            {
                int NB_TRY = instance.Start();
                "                        ".Draw(13, 7 + NB_TRY % 9);
                if (NB_TRY < 7) "Well Played!!!       ".Draw(13, 7 + NB_TRY % 9);
                else "Not Great..Try Again!".Draw(13, 7 + NB_TRY % 9);
            } while (Console.ReadKey(true).KeyChar == (char)ConsoleKey.Enter);
        }
    }

    public class MasterMind
    {

        /// <summary>
        /// Représente le nombre maximum de clef que l'IA peut gérer correctement.
        /// </summary>
        private int maxTable = 200000;
        private bool _lock = true;
        public void Unlock() { _lock = false; }
        public void SetMaxTable(int _maxTable){ if (!_lock) maxTable = _maxTable; }
        //============================PARAMETRES=====================================//
        private readonly Random Rand = new Random();
        /// <summary>
        ///Tableau contenant toutes les valeurs de TOKENS.
        /// </summary>
        private static readonly TOKEN[] TOKEN_VALUES = (TOKEN[])Enum.GetValues(typeof(TOKEN));
        /// <summary>
        ///Enumération des différents jetons possibles.
        /// </summary>
        public enum TOKEN { A, B, C, D, E, F, _ }
        /// <summary>
        /// PC/PLAYER ou TEST(sur J1, pour optimisation algo PC). J1 et J2 ont des rôles différents.
        /// </summary>
        public enum PARAM { PC, PLAYER, TEST }
        /// <summary>
        /// Taille de la clé secrète.
        /// </summary>
        public int KeySize;
        /// <summary>
        /// Espacement entre les tokens. Commence à 1.
        /// </summary>
        private readonly int SPACEMENT = 2;
        /// <summary>
        /// Position from left
        /// </summary>
        private readonly int X;
        /// <summary>
        /// Position from top
        /// </summary>
        private readonly int Y;
        /// <summary>
        /// le joueur 1 définie la clef. PARAM.PC ou PARAM.PLAYER, PARAM.TEST pour optimsation algo PC. 
        /// </summary>
        public PARAM Joueur_1;
        /// <summary>
        /// le joueur 2 trouve la clef. PARAM.PC ou PARAM.PLAYER. sur J2, PARAM.TEST équivaut uniquement à PARAM.PC.
        /// </summary>
        public PARAM Joueur_2;
        //============================VARIABLES=====================================//
        /// <summary>
        /// Clé secrète à trouver, définie par Joueur : Define_Key ou PC : Generate_Key.
        /// </summary>
        private readonly TOKEN[] Key;
        /// <summary>
        /// Proposition du joueur.
        /// </summary>
        private readonly TOKEN[] CurrentTrial;
        /// <summary>
        /// Indice du TOKEN selectionné.
        /// </summary>
        private int SelectedToken;
        /// <summary>
        /// Nombre d'essais.
        /// </summary>
        private int TrialsNumber;
        /// <summary>
        /// Ne pas confondre avec la Key. La CLEF est calculée sur la base des différences entre la dernière proposition et la clé secrète.
        /// regarder => "private int GetCLEF(TOKEN[] TRY, TOKEN[] KEY)" pour plus d'informations.
        /// </summary>
        public int CLEF;
        /// <summary>
        /// Liste générée si le joueur 2 est PC. Cette liste est générée par GENERATE_TABLE au début de la partie, et est ensuite progressivement supprimée par le PC. La dernière valeur est la solution.
        /// </summary>
        private readonly List<TOKEN[]> ALLKEY;

        //============================Constructeur=====================================//
        /// <summary>
        /// Créé une instance du jeu. new MasterMind().Start() pour lancer le jeu par défaut.
        /// </summary>
        /// <param name="joueur_1">Joueur qui défini la clef (PC/PLAYER). Definir J1 à TEST défini obligatoirement J2 à PC.</param>
        /// <param name="joueur_2">Joueur qui devine la clef (PC/PLAYER).</param>
        /// <param name="keySize">Taille de la clé à deviner.</param>
        /// <param name="X">pos from left.</param>
        /// <param name="Y">pos from top.</param>
        public MasterMind(PARAM joueur_1 = PARAM.PC, PARAM joueur_2 = PARAM.PLAYER, int keySize = 4, int X = 0, int Y = 0)
        {
            Joueur_1 = joueur_1;
            Joueur_2 = joueur_2;
            KeySize = keySize;
            if (Joueur_1 == PARAM.TEST) Joueur_2 = PARAM.PC;
            this.X = X;
            this.Y = Y;
            Key = new TOKEN[KeySize];
            CurrentTrial = new TOKEN[KeySize];
            ALLKEY = new List<TOKEN[]>();
        }
        //============================Boucle In Game=====================================//
        /// <summary>
        /// Demande/génére la clé secrète (Define_key / Generate_Key).
        /// Tant que la CLEF générée par la réponse ne correspond pas, le joueur 2 rejoue.
        /// </summary>
        /// <returns>Retourne le nombre d'essais pour trouver la clé</returns>
        public int Start()
        {
            TrialsNumber = 0;                                                                //#==Définition de la clé secrète==#
            ClearBoard();                                                                    //Dans le cas d'un joueur définissant la clé, 
            if (Joueur_1 == PARAM.PLAYER) Define_Key(); else Generate_Key();                 //on utilise la variable CurrentTrial pour recueillir temporairement celle ci.                                                                
            ClearBoard();
            do
            {                                                                              //le PARAM.TEST conditionne tous les affichages.
                if (Joueur_1 != PARAM.TEST) (1 + TrialsNumber).Draw(X, Y + TrialsNumber % 9);//TrialsNumber sert de "marqueur temporel".Il conditionne l'initialisation de la table et le premier coup du PC,
                if (Joueur_2 == PARAM.PLAYER) Input_Token();                               //il sert aussi a calculer la position de l'affichage avec un modulo 9.
                else PC_PLAY();                                                            //Input_Token pour le joueur, PC_PLAY sinon.     
                CLEF = GetCLEF(CurrentTrial, Key);                                              //La CLEF est utilisée pour deux raisons: 
                if (Joueur_1 != PARAM.TEST) DrawPlaced();                                       //-Elle économise des variables
                TrialsNumber++;                                                                 //-Elle permet de générer un dictionnaire pour l'IA (voir private int SimDispersion(TOKEN[] _IA_TRIAL))
                if ((TrialsNumber + 1) % 9 != 0) ClearLine();                                //Efface la ligne en dessous de l'entrée utilisateur, pour plus de visibilité.
            } while (CLEF != KeySize * (KeySize + 1));                                     //On boucle l'entrée de donnée utilisateur ou l'IA tant que la clé est incorrecte.
            return TrialsNumber;
        }
        //============================ALGORITHME DE DEDUCTION DE CLEF=====================================//
        /// <summary>
        /// ALGORITHME ITERATIF PAR METHODE BRUTE DE DEDUCTION.
        /// Génère une liste contenant toutes les clés secrètes possibles.
        /// Teste ensuite chaque clés avec toutes les autres, en stockant les CLEFS obtenues dans un dictionnaire.
        /// Selectionne ensuite la clé ayant générée le plus de CLEFS distinctes. On augmente ainsi les chances d'avoir une réponse très significative.
        /// Une fois la véritable CLEF récupérée, on teste la liste avec la réponse donnée précédement. Si la CLEF obtenue diffère de celle donnée, on supprime l'élément.
        /// On recommence le processus avec les clés restantes, jusqu'à n'avoir plus qu'une seule solution.
        /// </summary>
        private void PC_PLAY()
        {

            TOKEN[] IA_TRIAL = new TOKEN[KeySize];         //Clé temporaire utilisée par l'algorithme.                  
            if (Joueur_1 != PARAM.TEST) Draw_CurrentTrial();
            if (TrialsNumber == 0) GENERATE_TABLE();                                                                          //Génération complète de la table au premier tour de jeu.
            else for (int i = ALLKEY.Count - 1; i >= 0; i--) if (GetCLEF(CurrentTrial, ALLKEY[i]) != CLEF) ALLKEY.RemoveAt(i);//Ou supression de ces éléments à partir du second tour de jeu.


            if (ALLKEY.Count != 1)                                                                          //Si la liste ne contient qu'un élément, c'est cet élément qui est la solution.                                                     
            {
                int CurrentDispersion = 0;
                int NextDispersion;
                int max = TrialsNumber == 0 ? ABC_ToBase10(KeySize, TOKEN_VALUES.Length - 1) : ALLKEY.Count; //Lors du premier essai, ne teste que jusqu'à la clé A:B:C:D:X soit (1234..)en base (TOKEN_VALUES)
                for (int i = 0; i < max; i++)                                                                 //Toutes les autres clés étant sur le même modèle que les précédentes. (ex : 'b'cde équivaut 'a'cde au premier tour)
                {
                    NextDispersion = SimDispersion(ALLKEY[i]);                                                   //Simule toutes les clés
                    if (NextDispersion > CurrentDispersion)                                                      //Ne conserve que les clés donnant une meilleurs "dispersion" que celle précédement stockée.
                    {
                        CurrentDispersion = NextDispersion;                                                         //Actualise la meilleur dispersion.
                        for (int k = 0; k < KeySize; k++) IA_TRIAL[k] = ALLKEY[i][k];                               //Stock la clé temporaire ici
                    }
                }
            }
            for (int i = 0; i < KeySize; i++) CurrentTrial[i] = ALLKEY.Count == 1 ? ALLKEY[0][i] : IA_TRIAL[i];           //Donne la clé définitive.
            if (Joueur_1 != PARAM.TEST) Draw_CurrentTrial();                                                              //Affichage de la réponse.
        }
        /// <summary>
        /// Retourne le total de CLEFS différentes pour une clé, dans la liste de clé restante du PC.
        /// </summary>
        /// <param name="_IA_TRIAL">clé à tester</param>
        /// <returns>nombre total de CLEFS différentes pour cette clé</returns>
        private int SimDispersion(TOKEN[] _IA_TRIAL)
        {
            int CLEF;
            Dictionary<int, int> fractions = new Dictionary<int, int>();                                    //Le dictionnaire sert à distinguer les différentes clefs.
            int max = TrialsNumber == 0 ? ABC_ToBase10(KeySize, TOKEN_VALUES.Length - 1) : ALLKEY.Count;     //Lors du premier essai, ne teste que jusqu'à la clé A:B:C:D:X soit (1234..)en base (TOKEN_VALUES)
            for (int i = 0; i < max; i++)
            {
                CLEF = GetCLEF(_IA_TRIAL, ALLKEY[i]);                                                       //Récupère la CLEF
                if (fractions.TryGetValue(CLEF, out _)) fractions[CLEF]++;                                  //**Si la CLEF existe, on incrémente sa valeur.     (inutile pour le moment)                   
                else fractions.Add(CLEF, 1);                                                                //**Sinon, on ajoute la CLEF + valeur à 1       
            }
            return fractions.Count;                                                                          //**Retourne le nombre de clefs différentes dans le dictionnaire.
        }
        /// <summary>
        /// Calcul l'indice d'une clé "ABC..." dans la liste initiale avant modification de celle ci.
        /// </summary>
        /// <param name="Base">base numéraire représenté par le nombre de valeurs possibles d'un token (hors défaut)</param>
        /// <param name="Size">taille de la clef</param>
        /// <returns>retourne l'indice de la clé "ABC..." dans liste initiale.</returns>
        private int ABC_ToBase10(int Base, int Size)
        {
            int somme = 0; for (int i = 0; i < Size; i++) somme += (int)((Size - i - 1) * Math.Pow(Base, i)); return somme;  //Formule de conversion base => décimale.
        }
        /// <summary>
        /// Génère une liste de toutes les clés possible pour l'IA
        /// </summary>
        private void GENERATE_TABLE()
        {
            double NB = Math.Pow(TOKEN_VALUES.Length - 1, KeySize);
            if (NB > maxTable) throw new ArgumentException("IA can't handle this..", "TableKey"); //L'algorithme ne supporte pas au delà de 100 000/200 000 clefs.

            int j;
            int NBUToken = TOKEN_VALUES.Length - 1; //NomBre de Tokens Utilisables => Nombre de valeurs moins celle par défaut.
            ALLKEY.Clear();
            for (int i = 0; i < NB; i++)
            {
                j = i;
                ALLKEY.Add(new TOKEN[KeySize]); for (int k = KeySize - 1; k >= 0; k--)
                {
                    ALLKEY[i][k] = TOKEN_VALUES[j % NBUToken];
                    j /= NBUToken;
                }
            }
        }
        /// <summary>
        /// Génére une CLEF correspondant au nombre de tokens bien placés ou mal placés.
        /// </summary>
        /// <param name="TRY">Clé à tester</param>
        /// <param name="KEY">Clé à chercher</param>
        /// <returns>retourne la CLEF correspondant au nombre de tokens bien placés ou mal placés. </returns>
        private int GetCLEF(TOKEN[] TRY, TOKEN[] KEY)
        {
            int CLEF = 0;
            int A;
            int B;
            for (int i = 0; i < KeySize; i++)
            {
                if (TRY[i] == KEY[i]) CLEF += KeySize; //Calcul la partie représentant le nombre de "bien placée". Un "bien placé" induit un "mal placé", on retranche donc 1 à chaque "bien placé".
            }
            foreach (TOKEN C in TOKEN_VALUES) { A = TRY.GetCountOf(C); B = KEY.GetCountOf(C); CLEF += (A > B) ? B : A; }// Ajoute la partie représentant le nombre de "mal placés".  
            return CLEF;
        }
        //============================SAISIE ET AFFICHAGE=====================================//

        /// <summary>
        /// ClearBoard supprime les caractères dans la zone d'effet du jeu.
        /// </summary>
        private void ClearBoard()
        {
            if (Joueur_1 != PARAM.TEST) for (int i = 0; i < 9; i++) for (int j = 0; j < SPACEMENT * (KeySize + 1) + KeySize + 2; j++) " ".Draw(X + j, i + Y);
            SelectedToken = 0;
            Select_Current(1);
        }
        /// <summary>
        /// ClearLine ne supprime que la ligne en dessous de la saisie utilisateur. Elle est bouclée dans la zone d'effet du jeu avec un modulo sur TrialsNumber
        /// </summary>
        private void ClearLine()
        {
            for (int x = 0; x < SPACEMENT * (KeySize + 1) + KeySize + 2; x++) " ".Draw(X + x, ((TrialsNumber + 1) % 9) + Y);
        }

        /// <summary>
        /// Permet la saisie de la clé secrète par le joueur.
        /// </summary>
        private void Define_Key()
        {
            Input_Token();
            for (int i = 0; i < KeySize; i++) { Key[i] = CurrentTrial[i]; CurrentTrial[i] = TOKEN._; } //Define_Key() => Input_Token() => Key = CurrentTrial => Reinitialise CurrentTrial
        }
        /// <summary>
        /// Génère une clé secrète aléatoire, dans le cas d'une partie avec le PC en joueur 1.
        /// </summary>
        private void Generate_Key()
        {
            for (int i = 0; i < KeySize; i++)
            {
                Key[i] = TOKEN_VALUES[Rand.Next(TOKEN_VALUES.Length - 1)];
            }
        }
        /// <summary>
        /// Découpe la CLEF en deux partie : Well Placed et Misplaced et affiche le résultat, avec un 'O' pour chaque jeton WellPlaced et un 'X' pour chaque jeton Misplaced.
        /// </summary>
        private void DrawPlaced()
        {
            Console.SetCursorPosition(X + SPACEMENT * (KeySize + 1) + 2, Y + TrialsNumber);
            int i = CLEF / (KeySize + 1);
            while (i-- != 0) 'O'.Draw(-1);
            i = CLEF % (KeySize + 1);
            while (i-- != 0) 'X'.Draw(-1);
        }
        /// <summary>
        /// Module de saisie utilisateur: A,B,C,... 1,2,3,5 et enter/backspace. Enter pour valider.
        /// </summary>
        private void Input_Token()
        {
            for (int i = 0; i < KeySize; i++) CurrentTrial[i] = TOKEN._;  //Met la zone de saisie à vide
            Draw_CurrentTrial();                                          //Et la dessine
            char inputKey;
            do
            {
                inputKey = Console.ReadKey(true).KeyChar;           //Touches directionnelles non fonctionelles avec Console.ReadKey().. J'ai donc utilisé les touches numériques./
                switch (inputKey)                                   //## MAPPAGE DES TOUCHES:                                                                                                           
                {                                                   //-'a' 'b' 'c' 'd' 'e' 'f':                                                                                                                        
                    case '2':   Down_Current()  ;break;             //affecte la valeur au token et selectionne                                                                                                                                                                                          
                    case '5':   Up_Current()    ;break;             //le token suivant, sauf si celui ci est le dernier                                                                                                                         
                    case '1':   Left_Indice()   ;break;             //-backspace: recule de 1 token, sauf si celui ci est le premier                                                                                                                                                                                                         
                    case '3':   Right_Indice()  ;break;             //-enter: Valide la saisie, si celle ci est pleine.                                                                            
                                                                    //-'1' et '3':                                                                                                                 
                    case  (char)ConsoleKey.Backspace:               //Selectionne le token précédent ou le suivant. Arrivé à une extrémité, saute à l'extrémité opposée.                                                                                                                                                
                        if (SelectedToken > 0)                      //-'2' et '5':                                                                                                                                                                                                                         
                        Left_Indice();                              //Assigne la valeur précédente ou suivante au token selectionné. Une fois arrivé à une extrémité, saute à l'extrémité opposée.                                                                                                                      
                        break;                                      //La valeur de Token par défaut '_' est sauté.                                                                                                    
                }
                if (Enum.IsDefined(typeof(TOKEN), inputKey.ToString().ToUpper()))                   //Si la valeur saisie (ToUpper) existe dans l'enumération TOKEN, 
                    Set_Current((TOKEN)Enum.Parse(typeof(TOKEN), inputKey.ToString().ToUpper()));   //L'affecte au token selectionné. Set_Current appelle aussi Right_Indic.
            }
            while (CurrentTrial.Contains(TOKEN._) || (ConsoleKey)inputKey != ConsoleKey.Enter);     //Validation de la saisie.

        }
        /// <summary>
        /// Affiche la Saisie
        /// </summary>
        private void Draw_CurrentTrial()
        {
            for (SelectedToken = KeySize - 1; SelectedToken >= 0; SelectedToken--) Draw_Token();    // Parcours les tokens et les affiches.
            SelectedToken = 0;
            Select_Current(1);
        }
        /// <summary>
        /// Positionne le curseur sur le TOKEN selectionné.
        /// </summary>
        /// <param name="decalage">Décalage verticale du curseur. +=>Down -=Up</param>
        private void Select_Current(int decalage = 0)
        {
            Console.SetCursorPosition((SelectedToken + 1) * (SPACEMENT) + 1 + X, TrialsNumber % 9 + decalage + Y);
        }
        /// <summary>
        /// Défini la valeur du TOKEN selectionné.
        /// </summary>
        /// <param name="token">VALEUR assignée au token</param>
        private void Set_Current(TOKEN token)
        {
            CurrentTrial[SelectedToken] = token;
            Draw_Token();
            if (SelectedToken < KeySize-1)
            Right_Indice();
        }
        /// <summary>
        /// Affiche le token selectionné, puis place le curseur en dessous.
        /// </summary>
        private void Draw_Token()
        {
            Select_Current();
            CurrentTrial[SelectedToken].Draw(-1);
            Select_Current(1);
        }
        /// <summary>
        /// Modifie la valeur du token selectionné, par le haut.
        /// </summary>
        private void Up_Current()
        {
            CurrentTrial[SelectedToken] = CurrentTrial[SelectedToken].Up();
            if (CurrentTrial[SelectedToken] == TOKEN._) Up_Current();
            Draw_Token();
        }
        /// <summary>
        /// Modifie la valeur du token selectionné, par le bas.
        /// </summary>
        private void Down_Current()
        {
            CurrentTrial[SelectedToken] = CurrentTrial[SelectedToken].Down();
            if (CurrentTrial[SelectedToken] == TOKEN._) Down_Current();
            Draw_Token();
        }
        /// <summary>
        /// Change l'indice de sélection de token, par le bas.
        /// </summary>
        private void Left_Indice()
        {
            if (SelectedToken > 0) SelectedToken--;
            else SelectedToken = KeySize - 1;
            Select_Current(1);
        }
        /// <summary>
        /// Change l'indice de sélection de token, par le haut.
        /// </summary>
        private void Right_Indice()
        {
            if (SelectedToken < KeySize - 1) SelectedToken++;
            else SelectedToken = 0;
            Select_Current(1);
        }
    }
    public static class Extension
    {
        /// <summary>
        /// Méthode générique d'extension retournant l'indice d'une variable énumérée.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="src"></param>
        /// <returns>indice équivalent.</returns>
        public static int GetIndice<T>(this T src)
        {
            T[] Arr = (T[])Enum.GetValues(typeof(T));
            return Array.IndexOf(Arr, src);
        }
        /// <summary>
        /// Méthode générique d'extension retournant l'indice supérieur d'une variable énumérée.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="src"></param>
        /// <returns></returns>
        public static T Up<T>(this T src)
        {
            T[] Arr = (T[])Enum.GetValues(typeof(T));
            int j = Array.IndexOf(Arr, src) + 1;
            return (Arr.Length == j) ? Arr[0] : Arr[j];
        }
        /// <summary>
        /// Méthode générique d'extension retournant l'indice inférieur d'une variable énumérée.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="src"></param>
        /// <returns></returns>
        /// 
        public static T Down<T>(this T src)
        {
            T[] Arr = (T[])Enum.GetValues(typeof(T));
            int j = Array.IndexOf(Arr, src) - 1;
            return (j == -1) ? Arr[Arr.Length - 1] : Arr[j];
        }
        /// <summary>
        /// Méthode générique d'extension retournant le nombre d'occurances d'une variable énumérée dans un tableau d'énumérations.
        /// </summary>
        /// <param name="src">Tableau d'énumération</param>
        /// <param name="a">variable à chercher</param>
        /// <returns></returns>
        public static int GetCountOf(this MasterMind.TOKEN[] src, MasterMind.TOKEN a)
        {
            int c = 0;
            foreach (MasterMind.TOKEN b in src) if (a == b) c++;
            return c;
        }
        /// <summary>
        /// Méthode générique d'extension qui affiche l'objet fourni à la position donnée. Draw(-1) affichera l'objet à la position du curseur actuelle.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="to_draw">Objet à afficher</param>
        /// <param name="x">Position to the left. -1 => Affiche à la position actuelle.</param>
        /// <param name="y">Position to the top</param>
        public static void Draw<T>(this T to_draw, int x = 0, int y = 0)
        {
            if (x >= 0) Console.SetCursorPosition(x, y);
            Console.Write(to_draw);
        }

    }
}
