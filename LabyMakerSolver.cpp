//=> https://www.sfml-dev.org/tutorials/2.5/index-fr.php
#include <SFML/Graphics.hpp>


#include <cstdio>
#include <iostream>
#include <deque>
#include <ctime>


enum D { HAUT, BAS, DROITE, GAUCHE };
//Retourne le nombre de cases "creusées" autour de la case cible. Si la case cible est en bordure de map, retourne -1. La case est dite "libre" si le résultat est supérieur à 0.
int nbFreeArroundTarget(sf::Vector2u px, sf::Image& image);
//Fonction récursive. Retourne le nombre de cases non "libres", en partant d'une cible px, vers une direction d, et qui s'arrête de compter à partir d'une distance 'max'.
int nbUnfreeOnWayFromTarget(sf::Vector2u px, D d, sf::Image& image, int max = 1, int cpt = 0);
//Retourne, A partir des directions possibles (avec une distance non "libre" >0 ), une selection aléatoire de directions.
std::deque<D> getRandomsWays(sf::Vector2u px, sf::Image& image);
//"Creuse" à partir d'une direction px, direction d, distance distance. retourne la dernière case creusée.
sf::Vector2u digFromTarget(sf::Vector2u px, D d, int distance, sf::Image& image);
//Retourne une position proche de i qui soit entre les murs du labyrinthe. Retourn 0,0 s'il n'y en a pas, ou si la zone est en dehors du labyrinthe.
sf::Vector2u getPlot(sf::Vector2u i, sf::Image& image);
//Parcours le labyrinthe en sens inverse, de manière itérative, et retourne la case adjacente à celle donnée en départ la plus proche de la case d'arrivée. Affiche la recherche en transparence.
sf::Vector2u NextPixelFromStartToEnd(sf::Vector2u depart, sf::Vector2u arrivee, sf::Image& lab, sf::RenderWindow& window, sf::Sprite& sprite, sf::Texture& texture, sf::Image& image);
//Actualise l'affichage de la fenêtre.
void refresh(sf::RenderWindow& window, sf::Sprite& sprite, sf::Texture& texture, sf::Image& image);
//Renvoi vrai si la position est dans l'enceinte du labyrinthe, mur extérieurs exclus.
bool inlab(unsigned int x, unsigned int y);
//Renvoi la moyenne entre la couleur a et b, avec a coefficienté coefA
sf::Color average(sf::Color a, sf::Color b, float coefA);
//Taille maximum du labyrinthe
static sf::Vector2u max = { 200,200 };
//Coefficient d'affichage du labyrinthe
static int T = 4;
//Génèration du labyrinthe définie par M ou M est le nombre maximum de cases creusées en une fois entre deux choix d'intersections.
static int M = 9;
//Taux de rafraichissement. Le mode release est suffisement rapide pour être fluide en MODE 1. (max 4)
static int MODE = 1;
//Défini si le labyrinthe génére des sous branches jusqu'à remplir intégralement le damier.
static bool SUB = false;
int main(int charc, char* argv[]) {
	while (charc > 0) {
		charc--;
		std::string arg = argv[charc];
		if (arg.substr(0, 2)._Equal("x:"))
			max.x = std::stoi(arg.substr(2));
		if (arg.substr(0, 2)._Equal("y:"))
			max.y = std::stoi(arg.substr(2));
		if (arg.substr(0, 5)._Equal("size:"))
			T = std::stoi(arg.substr(5));
		if (arg.substr(0, 4)._Equal("gen:"))
			M = std::stoi(arg.substr(4));
		if (arg.substr(0, 4)._Equal("mode:"))
			MODE = std::stoi(arg.substr(5));
		if (arg._Equal("sub")) SUB = true;
		if (arg._Equal("nosub")) SUB = false;
	}
	//Permet d'activer le clic au relachement de la souris.
	bool pressed = false;
	//Génère le rand, bibliothèque ctime de la std.
	srand((int)time(NULL));
	//Déclaration de la fenêtre et des ses composants liés à la SFML.
	sf::RenderWindow window(sf::VideoMode(T * max.x, T * max.y), "Lab Maker", sf::Style::Close);
	sf::Image image;
	sf::Image icone;
	icone.loadFromFile("ico.png");
	window.setIcon(icone.getSize().x, icone.getSize().y,icone.getPixelsPtr());
	sf::Texture texture;
	sf::Sprite sprite;
	//Initialisation
	image.create(max.x, max.y, sf::Color::Black);
	texture.loadFromImage(image);
	sprite.setTexture(texture);
	sprite.setScale(sf::Vector2f((float)T, (float)T));
	refresh(window, sprite, texture, image);
	//Tableau de pixels contenant les "bourgeons" du labyrinthe.
	std::deque<sf::Vector2u> pix;
	sf::Event event;
	//Gère les evenements en attendant la selection d'un "bourgeon".
	while (window.isOpen() && !pix.size()) {
		while (window.pollEvent(event))
		{
			//Fermeture du programme.
			if (event.type == sf::Event::Closed) { window.close(); return 0; }
			// Clic souris.
			if (sf::Mouse::isButtonPressed(sf::Mouse::Left)) pressed = true;
			// Relachement souris.
			else if (pressed) {
				//Récupère la position relative à la fenêtre de la souris.
				sf::Vector2i clic = sf::Mouse::getPosition(window);
				clic.x /= T;
				clic.y /= T;
				//Si la zone selectionnée est dans le labyrinthe, pose un "bourgeons".
				if (inlab(clic.x, clic.y))pix.push_back({ (unsigned int)clic.x,(unsigned int)clic.y });
				//  clic+relachement souris traité.
				pressed = false;
			}
		}
	}
	//On "dessine" le point de départ ("bourgeon")
	image.setPixel(pix[0].x, pix[0].y, sf::Color::White);
	do {
		//Tant qu'il y'a des bourgeons à traiter
		while (pix.size()) {
			//Récupère la taille de la génération N
			int SIZE = pix.size();
			//Traite tous les bourgeons de la générations N
			for (int i = 0; i < SIZE; i++) {
				//Récupère la sélection aléatoires de chemins pour chaque bourgeon de la génération N
				std::deque<D> way = getRandomsWays(pix[i], image);
				for (int j = 0; j < (int)way.size(); j++) {
					//Creuse dans le labyrinthe et ajoute la génération N+1 de bourgeons au labyrinthe. Le labyrinthe a donc le format [(génération N)+(génération N+1)]
					pix.push_back(digFromTarget(pix[i], way[j], rand() % nbUnfreeOnWayFromTarget(pix[i], way[j], image, M) + 1, image));
					//Affichage mode 1
					if (MODE == 1) refresh(window, sprite, texture, image);
				}
				//Affichage mode 2
				if (MODE == 2) refresh(window, sprite, texture, image);
			}
			//Affichage mode 3
			if (MODE == 3) refresh(window, sprite, texture, image);
			//Détruit la génération N
			while (SIZE--) {
				pix.erase(pix.begin());
			}
			//Gère les evenements
			while (window.pollEvent(event)) {
				//Fermeture de l'application
				if (event.type == sf::Event::Closed) { window.close(); return 0; }
			}
		}
		//Sous branches du labyrinthe, si l'option est activée.
		if (SUB) {
			//Curseur pour parcourir l'image
			sf::Vector2u i;
			//Tableau vide
			std::deque<sf::Vector2u> randsPX;
			//Parcours l'image et remplis le tableau avec les endroits pouvant acceuillir un bourgeon.
			for (i.x = 1; i.x < max.x - 1; i.x++)
				for (i.y = 1; i.y < max.y - 1; i.y++)
				{
					//Si le curseur est en dehors du labyrinthe, et est collé au labyrinthe en un seul point, il est ajouté au tableau.
					if (nbFreeArroundTarget(i, image) == 1 && image.getPixel(i.x, i.y) != sf::Color::White) {
						randsPX.push_back(i);
					}
				}
			//Si il existe des "endroits" dans le labyrinthe pouvant accueillir, choisi aléatoirement dans la liste le point de départ du bourgeon.
			if (randsPX.size() && SUB) {
				i = randsPX[rand() % randsPX.size()];
				//Ajoute du bourgeon sur l'image
				image.setPixel(i.x, i.y, sf::Color::White);
				//Ajout du bourgeon dans le tableau 
				pix.push_back(i);
				if (MODE == 1) refresh(window, sprite, texture, image);
			}
		}
		//Continu, tant qu'il y'a des endroits ou poser des bourgeons.
	} while (pix.size());
	refresh(window, sprite, texture, image);
	//Duplique le labyrinte, pour pouvoir régénerer le solveur après chaque passage de l'algorithme, tout en gardant l'affichage principal.
	//Image de travail
	sf::Image lab;
	lab.create(max.x, max.y, sf::Color(0, 0, 0));
	//Image de backup
	sf::Image backuplab;
	backuplab.create(max.x, max.y, sf::Color(0, 0, 0));
	backuplab.copy(image, 0, 0);
	int plot = 0;//Peut avoir 3 valeurs: 0, 1 ou 2. 1: Départ posé, 2:Arrivée posée.
	//Génère une couleur aléatoire pour le tracé.
	sf::Color couleur(rand() % 256, rand() % 256, rand() % 256);
	//Plot de départ et d'arrivée.
	sf::Vector2u _depart;
	sf::Vector2u _arrivee;
	//Gestion des événements.
	while (window.isOpen()) {
		while (window.pollEvent(event))
		{
			//Fermeture du programme.
			if (event.type == sf::Event::Closed) { window.close(); return 0; }
			// Clic souris.
			if (sf::Mouse::isButtonPressed(sf::Mouse::Left)) pressed = true;
			// Relachement souris.
			else if (pressed) {
				pressed = false;
				sf::Vector2i clic = sf::Mouse::getPosition(window);
				clic.x /= T;
				clic.y /= T;
				if (inlab(clic.x, clic.y)) {
					if (plot == 0) {
						_depart = getPlot({ (unsigned int)clic.x, (unsigned int)clic.y }, image);
						if (_depart.x > 0) {
							plot++;
							image.setPixel(_depart.x, _depart.y, couleur);
						}
					}
					else {
						_arrivee = getPlot({ (unsigned int)clic.x, (unsigned int)clic.y }, image);
						if (_arrivee.x > 0) {
							plot++;
							image.setPixel(_arrivee.x, _arrivee.y, couleur);
						}
					}
				}
				refresh(window, sprite, texture, image);
			}
			//Si les deux plots sont placés, le solveur se lance.
			if (plot == 2) {
				//Récupère le labyrinthe propre pour le traiter.
				lab.copy(backuplab, 0, 0);
				//Déplace le curseur depart jusqu'à l'arrivée.
				while (_depart.x != _arrivee.x || _depart.y != _arrivee.y) {
					//Déplace le curseur d'une case, affiche la recherche.
					_depart = NextPixelFromStartToEnd(_depart, _arrivee, lab, window, sprite, texture, image);
					//Affiche le curseur sur l'image.
					image.setPixel(_depart.x, _depart.y, couleur);
					refresh(window, sprite, texture, image);
					//A chaque déplacement du curseur, réinitialise l'image de travail.
					lab.copy(backuplab, 0, 0);
				}
				// Remet le compteur de plot à 0.
				plot = 0;
				//Selectionne une nouvelle couleur pour le prochain trajet.
				couleur = sf::Color(rand() % 256, rand() % 256, rand() % 256);
			}
		}
	}
	return 0;
}
int nbUnfreeOnWayFromTarget(sf::Vector2u px, D d, sf::Image& image, int max, int cpt) {
	if (cpt >= max) return cpt;
	switch (d) {
	case HAUT:
		px.y--;
		break;
	case BAS:
		px.y++;
		break;
	case DROITE:
		px.x++;
		break;
	case GAUCHE:
		px.x--;
		break;
	}
	int nb = nbFreeArroundTarget(px, image);
	if (nb == -1) return cpt;
	if (cpt == 0 && nb == 1) return nbUnfreeOnWayFromTarget(px, d, image, max, cpt + 1);
	if (cpt > 0 && nb == 0) return nbUnfreeOnWayFromTarget(px, d, image, max, cpt + 1);
	return cpt;
}
std::deque<D> getRandomsWays(sf::Vector2u px, sf::Image& image) {
	std::deque<D> way;
	if (nbUnfreeOnWayFromTarget(px, HAUT, image) == 1) way.push_back(HAUT);
	if (nbUnfreeOnWayFromTarget(px, BAS, image) == 1) way.push_back(BAS);
	if (nbUnfreeOnWayFromTarget(px, DROITE, image) == 1) way.push_back(DROITE);
	if (nbUnfreeOnWayFromTarget(px, GAUCHE, image) == 1) way.push_back(GAUCHE);
	if (way.empty()) return way;
	int nb = rand() % way.size();
	while (nb) {
		way.erase(way.begin() + rand() % way.size());
		nb--;
	}
	return way;
}
int nbFreeArroundTarget(sf::Vector2u px, sf::Image& image) {
	if (!inlab(px.x, px.y)) return -1;
	int cpt = 0;
	if (inlab(px.x + 1, px.y) && image.getPixel(px.x + 1, px.y) == sf::Color::White) cpt++;
	if (inlab(px.x - 1, px.y) && image.getPixel(px.x - 1, px.y) == sf::Color::White) cpt++;
	if (inlab(px.x, px.y + 1) && image.getPixel(px.x, px.y + 1) == sf::Color::White) cpt++;
	if (inlab(px.x, px.y - 1) && image.getPixel(px.x, px.y - 1) == sf::Color::White) cpt++;
	return cpt;
}
bool inlab(unsigned int x, unsigned int y) {
	if (x <= 0 || y <= 0 || x >= max.x - 1 || y >= max.y - 1) return false;
	return true;

}
sf::Vector2u digFromTarget(sf::Vector2u px, D d, int distance, sf::Image& image) {
	while (distance--) {
		switch (d) {
		case HAUT:
			px.y--;
			break;
		case BAS:
			px.y++;
			break;
		case DROITE:
			px.x++;
			break;
		case GAUCHE:
			px.x--;
			break;
		}
		image.setPixel(px.x, px.y, sf::Color(255, 255, 255));
	}
	return px;
}
void refresh(sf::RenderWindow& window, sf::Sprite& sprite, sf::Texture& texture, sf::Image& image) {
	texture.update(image);
	window.clear();
	window.draw(sprite);
	window.display();
}
sf::Vector2u getPlot(sf::Vector2u i, sf::Image& image) {
	if (image.getPixel(i.x, i.y) != sf::Color(0, 0, 0)) return i;
	sf::Vector2u px;
	for (px.x = i.x - 1; px.x < i.x + 1; px.x++)
		for (px.y = i.y - 1; px.y < i.y + 1; px.y++) {
			if (inlab(px.x, px.y) && image.getPixel(px.x, px.y) != sf::Color::Black) return px;
		}
	return { 0,0 };
}
sf::Vector2u NextPixelFromStartToEnd(sf::Vector2u depart, sf::Vector2u arrivee, sf::Image& lab, sf::RenderWindow& window, sf::Sprite& sprite, sf::Texture& texture, sf::Image& image) {
	sf::Vector2u curseur;
	sf::Color color;
	sf::Color colorlab = image.getPixel(depart.x, depart.y);
	std::deque<sf::Vector2u> splitcurseur;
	splitcurseur.push_back({ arrivee.x,arrivee.y });
	if (arrivee.x == depart.x + 1 && arrivee.y == depart.y) return arrivee;
	if (arrivee.x == depart.x - 1 && arrivee.y == depart.y) return arrivee;
	if (arrivee.x == depart.x && arrivee.y == depart.y + 1) return arrivee;
	if (arrivee.x == depart.x && arrivee.y == depart.y - 1) return arrivee;

	while (true) {
		int size = splitcurseur.size();
		for (int i = 0; i < size; i++) {
			curseur = splitcurseur[i];
			if (curseur.x + 1 == depart.x && curseur.y == depart.y) return splitcurseur[i];
			if (curseur.x - 1 == depart.x && curseur.y == depart.y) return splitcurseur[i];
			if (curseur.x == depart.x && curseur.y + 1 == depart.y) return splitcurseur[i];
			if (curseur.x == depart.x && curseur.y - 1 == depart.y) return splitcurseur[i];
			curseur.x++;
			color = lab.getPixel(curseur.x, curseur.y);
			if (color != sf::Color::Black && color != colorlab) {
				splitcurseur.push_back(curseur);
				lab.setPixel(curseur.x, curseur.y, colorlab);
				image.setPixel(curseur.x, curseur.y, average(colorlab, image.getPixel(curseur.x, curseur.y), 0.01f));
			}
			curseur.x -= 2;
			color = lab.getPixel(curseur.x, curseur.y);
			if (color != sf::Color::Black && color != colorlab) {
				splitcurseur.push_back(curseur);
				lab.setPixel(curseur.x, curseur.y, colorlab);
				image.setPixel(curseur.x, curseur.y, average(colorlab, image.getPixel(curseur.x, curseur.y), 0.01f));
			}
			curseur.x++;
			curseur.y++;
			color = lab.getPixel(curseur.x, curseur.y);
			if (color != sf::Color::Black && color != colorlab) {
				splitcurseur.push_back(curseur);
				lab.setPixel(curseur.x, curseur.y, colorlab);
				image.setPixel(curseur.x, curseur.y, average(colorlab, image.getPixel(curseur.x, curseur.y), 0.01f));
			}
			curseur.y -= 2;
			color = lab.getPixel(curseur.x, curseur.y);
			if (color != sf::Color::Black && color != colorlab) {
				splitcurseur.push_back(curseur);
				lab.setPixel(curseur.x, curseur.y, colorlab);
				image.setPixel(curseur.x, curseur.y, average(colorlab, image.getPixel(curseur.x, curseur.y), 0.01f));
			}
		}
		for (int i = 0; i < size; i++) {
			splitcurseur.erase(splitcurseur.begin());
		}
	}

}
sf::Color average(sf::Color a, sf::Color b, float coefA) {
	sf::Color color;
	color.r = (sf::Uint8)((coefA * a.r + b.r) / (1 + coefA));
	color.b = (sf::Uint8)((coefA * a.b + b.b) / (1 + coefA));
	color.g = (sf::Uint8)((coefA * a.g + b.g) / (1 + coefA));
	return color;
}
