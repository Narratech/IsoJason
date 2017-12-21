using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoldMinersMaps : MonoBehaviour{

    public void World1() {
        GoldMinersWorld world = GetComponent<GoldMinersWorld>();
        world.Initialize(21, 21, 4);
        world.SetDepot(5, 7);
        world.SetAgPos(0, 1, 0);
        world.SetAgPos(1, 20, 0);
        world.SetAgPos(2, 3, 20);
        world.SetAgPos(3, 20, 20);
        world.SetInitialNbGolds(world.CountObjects(GoldMinersWorld.GOLD));
    }

    /** un escenario con oro pero sin obstáculos */
    public void World2() {
        GoldMinersWorld world = GetComponent<GoldMinersWorld>();
        world.Initialize(35, 35, 4);
        world.SetDepot(5, 27);
        world.SetAgPos(0, 1, 0);
        world.SetAgPos(1, 20, 0);
        world.SetAgPos(2, 3, 20);
        world.SetAgPos(3, 20, 20);
        world.Add(GoldMinersWorld.GOLD, 20, 13);
        world.Add(GoldMinersWorld.GOLD, 15, 20);
        world.Add(GoldMinersWorld.GOLD, 1, 1);
        world.Add(GoldMinersWorld.GOLD, 3, 5);
        world.Add(GoldMinersWorld.GOLD, 24, 24);
        world.Add(GoldMinersWorld.GOLD, 20, 20);
        world.Add(GoldMinersWorld.GOLD, 20, 21);
        world.Add(GoldMinersWorld.GOLD, 20, 22);
        world.Add(GoldMinersWorld.GOLD, 20, 23);
        world.Add(GoldMinersWorld.GOLD, 20, 24);
        world.Add(GoldMinersWorld.GOLD, 19, 20);
        world.Add(GoldMinersWorld.GOLD, 19, 21);
        world.Add(GoldMinersWorld.GOLD, 34, 34);
        world.SetInitialNbGolds(world.CountObjects(GoldMinersWorld.GOLD));
    }

    /** el escenario 3, el más jugoso, con oro y obstáculos a saco */
    public void World3() {
        GoldMinersWorld world = GetComponent<GoldMinersWorld>();
        world.Initialize(35, 35, 4);
        world.SetDepot(16, 16);
        world.SetAgPos(0, 1, 0);
        world.SetAgPos(1, 20, 0);
        world.SetAgPos(2, 6, 26);
        world.SetAgPos(3, 20, 20);
        world.Add(GoldMinersWorld.GOLD, 20, 13);
        world.Add(GoldMinersWorld.GOLD, 15, 20);
        world.Add(GoldMinersWorld.GOLD, 1, 1);
        world.Add(GoldMinersWorld.GOLD, 3, 5);
        world.Add(GoldMinersWorld.GOLD, 24, 24);
        world.Add(GoldMinersWorld.GOLD, 20, 20);
        world.Add(GoldMinersWorld.GOLD, 20, 21);
        world.Add(GoldMinersWorld.GOLD, 2, 22);
        world.Add(GoldMinersWorld.GOLD, 2, 12);
        world.Add(GoldMinersWorld.GOLD, 19, 2);
        world.Add(GoldMinersWorld.GOLD, 14, 4);
        world.Add(GoldMinersWorld.GOLD, 34, 34);

        world.Add(GoldMinersWorld.OBSTACLE, 12, 3);
        world.Add(GoldMinersWorld.OBSTACLE, 13, 3);
        world.Add(GoldMinersWorld.OBSTACLE, 14, 3);
        world.Add(GoldMinersWorld.OBSTACLE, 15, 3);
        world.Add(GoldMinersWorld.OBSTACLE, 18, 3);
        world.Add(GoldMinersWorld.OBSTACLE, 19, 3);
        world.Add(GoldMinersWorld.OBSTACLE, 20, 3);
        world.Add(GoldMinersWorld.OBSTACLE, 14, 8);
        world.Add(GoldMinersWorld.OBSTACLE, 15, 8);
        world.Add(GoldMinersWorld.OBSTACLE, 16, 8);
        world.Add(GoldMinersWorld.OBSTACLE, 17, 8);
        world.Add(GoldMinersWorld.OBSTACLE, 19, 8);
        world.Add(GoldMinersWorld.OBSTACLE, 20, 8);

        world.Add(GoldMinersWorld.OBSTACLE, 12, 32);
        world.Add(GoldMinersWorld.OBSTACLE, 13, 32);
        world.Add(GoldMinersWorld.OBSTACLE, 14, 32);
        world.Add(GoldMinersWorld.OBSTACLE, 15, 32);
        world.Add(GoldMinersWorld.OBSTACLE, 18, 32);
        world.Add(GoldMinersWorld.OBSTACLE, 19, 32);
        world.Add(GoldMinersWorld.OBSTACLE, 20, 32);
        world.Add(GoldMinersWorld.OBSTACLE, 14, 28);
        world.Add(GoldMinersWorld.OBSTACLE, 15, 28);
        world.Add(GoldMinersWorld.OBSTACLE, 16, 28);
        world.Add(GoldMinersWorld.OBSTACLE, 17, 28);
        world.Add(GoldMinersWorld.OBSTACLE, 19, 28);
        world.Add(GoldMinersWorld.OBSTACLE, 20, 28);

        world.Add(GoldMinersWorld.OBSTACLE, 3, 12);
        world.Add(GoldMinersWorld.OBSTACLE, 3, 13);
        world.Add(GoldMinersWorld.OBSTACLE, 3, 14);
        world.Add(GoldMinersWorld.OBSTACLE, 3, 15);
        world.Add(GoldMinersWorld.OBSTACLE, 3, 18);
        world.Add(GoldMinersWorld.OBSTACLE, 3, 19);
        world.Add(GoldMinersWorld.OBSTACLE, 3, 20);
        world.Add(GoldMinersWorld.OBSTACLE, 8, 14);
        world.Add(GoldMinersWorld.OBSTACLE, 8, 15);
        world.Add(GoldMinersWorld.OBSTACLE, 8, 16);
        world.Add(GoldMinersWorld.OBSTACLE, 8, 17);
        world.Add(GoldMinersWorld.OBSTACLE, 8, 19);
        world.Add(GoldMinersWorld.OBSTACLE, 8, 20);

        world.Add(GoldMinersWorld.OBSTACLE, 32, 12);
        world.Add(GoldMinersWorld.OBSTACLE, 32, 13);
        world.Add(GoldMinersWorld.OBSTACLE, 32, 14);
        world.Add(GoldMinersWorld.OBSTACLE, 32, 15);
        world.Add(GoldMinersWorld.OBSTACLE, 32, 18);
        world.Add(GoldMinersWorld.OBSTACLE, 32, 19);
        world.Add(GoldMinersWorld.OBSTACLE, 32, 20);
        world.Add(GoldMinersWorld.OBSTACLE, 28, 14);
        world.Add(GoldMinersWorld.OBSTACLE, 28, 15);
        world.Add(GoldMinersWorld.OBSTACLE, 28, 16);
        world.Add(GoldMinersWorld.OBSTACLE, 28, 17);
        world.Add(GoldMinersWorld.OBSTACLE, 28, 19);
        world.Add(GoldMinersWorld.OBSTACLE, 28, 20);

        world.Add(GoldMinersWorld.OBSTACLE, 13, 13);
        world.Add(GoldMinersWorld.OBSTACLE, 13, 14);

        world.Add(GoldMinersWorld.OBSTACLE, 13, 16);
        world.Add(GoldMinersWorld.OBSTACLE, 13, 17);

        world.Add(GoldMinersWorld.OBSTACLE, 13, 19);
        world.Add(GoldMinersWorld.OBSTACLE, 14, 19);

        world.Add(GoldMinersWorld.OBSTACLE, 16, 19);
        world.Add(GoldMinersWorld.OBSTACLE, 17, 19);

        world.Add(GoldMinersWorld.OBSTACLE, 19, 19);
        world.Add(GoldMinersWorld.OBSTACLE, 19, 18);

        world.Add(GoldMinersWorld.OBSTACLE, 19, 16);
        world.Add(GoldMinersWorld.OBSTACLE, 19, 15);

        world.Add(GoldMinersWorld.OBSTACLE, 19, 13);
        world.Add(GoldMinersWorld.OBSTACLE, 18, 13);

        world.Add(GoldMinersWorld.OBSTACLE, 16, 13);
        world.Add(GoldMinersWorld.OBSTACLE, 15, 13);

        // labirinto
        world.Add(GoldMinersWorld.OBSTACLE, 2, 32);
        world.Add(GoldMinersWorld.OBSTACLE, 3, 32);
        world.Add(GoldMinersWorld.OBSTACLE, 4, 32);
        world.Add(GoldMinersWorld.OBSTACLE, 5, 32);
        world.Add(GoldMinersWorld.OBSTACLE, 6, 32);
        world.Add(GoldMinersWorld.OBSTACLE, 7, 32);
        world.Add(GoldMinersWorld.OBSTACLE, 8, 32);
        world.Add(GoldMinersWorld.OBSTACLE, 9, 32);
        world.Add(GoldMinersWorld.OBSTACLE, 10, 32);
        world.Add(GoldMinersWorld.OBSTACLE, 10, 31);
        world.Add(GoldMinersWorld.OBSTACLE, 10, 30);
        world.Add(GoldMinersWorld.OBSTACLE, 10, 29);
        world.Add(GoldMinersWorld.OBSTACLE, 10, 28);
        world.Add(GoldMinersWorld.OBSTACLE, 10, 27);
        world.Add(GoldMinersWorld.OBSTACLE, 10, 26);
        world.Add(GoldMinersWorld.OBSTACLE, 10, 25);
        world.Add(GoldMinersWorld.OBSTACLE, 10, 24);
        world.Add(GoldMinersWorld.OBSTACLE, 10, 23);
        world.Add(GoldMinersWorld.OBSTACLE, 2, 23);
        world.Add(GoldMinersWorld.OBSTACLE, 3, 23);
        world.Add(GoldMinersWorld.OBSTACLE, 4, 23);
        world.Add(GoldMinersWorld.OBSTACLE, 5, 23);
        world.Add(GoldMinersWorld.OBSTACLE, 6, 23);
        world.Add(GoldMinersWorld.OBSTACLE, 7, 23);
        world.Add(GoldMinersWorld.OBSTACLE, 8, 23);
        world.Add(GoldMinersWorld.OBSTACLE, 9, 23);
        world.Add(GoldMinersWorld.OBSTACLE, 2, 29);
        world.Add(GoldMinersWorld.OBSTACLE, 2, 28);
        world.Add(GoldMinersWorld.OBSTACLE, 2, 27);
        world.Add(GoldMinersWorld.OBSTACLE, 2, 26);
        world.Add(GoldMinersWorld.OBSTACLE, 2, 25);
        world.Add(GoldMinersWorld.OBSTACLE, 2, 24);
        world.Add(GoldMinersWorld.OBSTACLE, 2, 23);
        world.Add(GoldMinersWorld.OBSTACLE, 2, 29);
        world.Add(GoldMinersWorld.OBSTACLE, 3, 29);
        world.Add(GoldMinersWorld.OBSTACLE, 4, 29);
        world.Add(GoldMinersWorld.OBSTACLE, 5, 29);
        world.Add(GoldMinersWorld.OBSTACLE, 6, 29);
        world.Add(GoldMinersWorld.OBSTACLE, 7, 29);
        world.Add(GoldMinersWorld.OBSTACLE, 7, 28);
        world.Add(GoldMinersWorld.OBSTACLE, 7, 27);
        world.Add(GoldMinersWorld.OBSTACLE, 7, 26);
        world.Add(GoldMinersWorld.OBSTACLE, 7, 25);
        world.Add(GoldMinersWorld.OBSTACLE, 6, 25);
        world.Add(GoldMinersWorld.OBSTACLE, 5, 25);
        world.Add(GoldMinersWorld.OBSTACLE, 4, 25);
        world.Add(GoldMinersWorld.OBSTACLE, 4, 26);
        world.Add(GoldMinersWorld.OBSTACLE, 4, 27);
        world.SetInitialNbGolds(world.CountObjects(GoldMinersWorld.GOLD));
    }


}
