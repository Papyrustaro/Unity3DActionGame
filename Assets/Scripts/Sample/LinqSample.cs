using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

// https://www.hanachiru-blog.com/entry/2019/03/27/213020
public class LinqSample : MonoBehaviour
{
    private void Start()
    {
        //OrderBySample();
    }

    private void WhereSample()
    {
        IEnumerable<int> numbers = Enumerable.Range(1, 5)
            .Where(i => i <= 3);

        foreach(int num in numbers)
        {
            Debug.Log(num); 
        }
    }

    private void SelectSample()
    {
        IEnumerable<int> numbers = Enumerable.Range(1, 5)
            .Select(n => n * n);

        foreach(int num in numbers)
        {
            Debug.Log(num);
        }
    }

    private void DistinctSample()
    {
        List<int> numbers = new List<int> { 1, 2, 3, 3, 4, 4 };
        IEnumerable<int> results = numbers.Distinct();
        foreach (int num in results) Debug.Log(num);
    }

    private void OrderBySample()
    {
        List<Enemy> enemies = new List<Enemy>
        {
            new Enemy{Name = "スライム", Hp = 20, Exp = 10},
            new Enemy{Name = "メタルスライム", Hp = 30, Exp = 10 },
            new Enemy{Name = "はぐれメタル", Hp = 20, Exp = 10 },
        };

        IEnumerable<Enemy> sortedEnemy = enemies.OrderBy(x => x.Hp);
        foreach (Enemy enemy in sortedEnemy) Debug.Log(enemy.Name);
        foreach (Enemy enemy in enemies.OrderBy(x => x.Hp)) Debug.Log(enemy.Name);
    }

    private void ConcatSample()
    {
        List<int> numbers1 = new List<int> { 1, 2, 3 };
        List<int> numbers2 = new List<int> { 4, 3, 2 };
        IEnumerable<int> concatedNumber = numbers1.Concat(numbers2);

        foreach (int num in concatedNumber) Debug.Log(num);
    }

    private void RepeatSample()
    {
        IEnumerable<int> numbers = Enumerable.Repeat(-1, 5);
        foreach (int num in numbers) Debug.Log(num);
    }

    private void RangeSample()
    {
        IEnumerable<int> numbers = Enumerable.Range(1, 5);
        foreach (int num in numbers) Debug.Log(num);
    }

    private void ToListSample()
    {
        List<int> numbers = Enumerable.Repeat(-1, 20).ToList();
    }
}

public class Enemy
{
    public string Name { get; set; }
    public int Hp { get; set; }
    public int Exp { get; set; }
}
