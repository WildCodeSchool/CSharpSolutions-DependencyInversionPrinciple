# 3. SOLID - Dependency Inversion Principle

*Après cette quête tu sauras reconnaître une violation du principe d'inversion de dépendances et sauras y remédier.*

## Objectifs

* Comprendre la notion d'inversion de dépendance
* Reconnaître une violation du principe
* Corriger une violation
* Concevoir du code en appliquant le principe
* Appliquer un design pattern

## Etapes

### Des briques communiquantes

Un programme, dès lors qu'il est suffisamment imposant en terme de fonctionnalités et de code à produire, est souvent découpé en modules ayant chacun un rôle particulier dans tout le programme.

Par exemple, un site Web permettant à des utilisateurs de s'inscrire pour acheter ou vendre des chats a besoin de se connecter à une base de données pour y stocker et y retrouver les informations concernant les utilisateurs et les chats à vendre. La base de données est un module à part, il s'agit même de la brique fondamentale car sans elle le site n'a aucun interêt réel ! 

Cependant, le code qui utilisera le module base de données sera très fortement lié au module en question. A un tel point qu'il ne sera plus possible de changer le code du module de la base de données sans changer le ou les modules qui en dépendent. Cela amplifie la **dette technique** et donc ralentit les temps de développement, et cela s'empire au fur et à mesure, tout cela car il y a un **couplage fort** entre les différents modules: si une classe dans un module éprouve une faiblesse, tout le programme est impacté donc sa robustesse est faible.

![Diagramme de modules](https://www.visual-paradigm.com/VPGallery/img/diagrams/Package/Package-Diagram-Sample.png)

> N'y aurait-il pas une méthode pour réduire le couplage et rendre chaque partie indépendante ?

#### Ressources

* [Wikipédia - Dette technique](https://fr.wikipedia.org/wiki/Dette_technique) A lire jusqu'au bout 
* [Cours sur cohérence et couplage](https://www.youtube.com/watch?v=ngdWrYIY5Jg) Du 5:30 à la fin. Lire à partir du début est cependant fortement recommandé pour comprendre la notion de cohérence.

### Le principe théorique

Afin de réduire le couplage entre les  classes, Robert C. Martin (alias Oncle Bob), un informaticien très fameux pour ses livres et interventions sur le code propre et la méthode agile, a mis au grand jour plusieurs principes dont celui de l'*inversion de dépendance*.

Oncle Bob a postulé les deux affirmations suivantes comme étant la base du principe d'*inversion de dépendance*:

* Les modules de haut-niveau ne doivent pas dépendre de modules de bas niveau. Les deux doivent dépendre d'abstractions.
* Les abstractions ne doivent pas dépendre des détails. Les détails doivent dépendre des abstractions.

> C'est pas logique ! Le code construit au-dessus d'un autre dépend **forcément** de lui. Et pourquoi des *abstractions* ?

Le principe en tant que tel n'est pas forcément évident à mettre en oeuvre si nous ne disposons pas d'une façon de faire nous permettant de réduire le couplage entre les modules. L'image suivante nous permettra de mieux comprendre le principe:

#### Ressources

### Un cas réel

![DIP Violation](https://deviq.com/wp-content/uploads/2014/11/DependencyInversion.jpg)

L'image ci-dessus est plutôt démonstrative du principe. On voit des fils d'un câble électrique **directement** soudés à une prise électrique. C'est complètement absurde car si je veux brancher autre chose à la prise, je vais être obligé de dessouder les fils pour en souder un nouveau, ce qui me fait perdre beaucoup de temps par rapport à un simple branchement de prise. Qui plus est c'est une opération pouvant s'avérer dangereuse pour ma vie.

Pour éviter d'avoir à souder/dessouder des fils à une prise à chaque changement d'appareil à relier au réseau électrique, nous utilisons communément des **interfaces**. Eh oui, la fiche (la partie à rentrer dans une prise) du câble d'un appareil électronique est une **interface** permettant de connecter ledit appareil au réseau électrique. Le réseau électrique se satisfait bien d'une simple fiche et ne nécessite pas que les fils de l'appareil soient directement *soudés* sur le réseau. Au niveau conceptuel, on peut considérer une prise comme une **interface** permettant de connecter un appareil électrique électronique au réseau.

> Mais on fait du code, pas de l'électro-technique ... pas vrai ?

En code c'est exactement le même principe: dans un module haut-niveau, plutôt que de directement dépendre d'une classe d'un module du niveau d'en-dessous, on utilise une **interface** vers le module sous-jacent directement. De cette manière, le programme final est bien plus modulable car le couplage entre les modules de différents niveaux est extrêment réduit: il est donc possible d'apporter des modifications à un module en n'impactant pas les modules sus-jacents. Cela nous fait donc gagner du temps, facilite les modifications et les livraisons de nouvelles versions d'un programme, tout en limitant les bugs dû à un couplage important. 

> Ok, mais comment je met ça en place dans mon code, ça a l'air compliqué !

#### Ressources

* [Dependency Inversion Principle - Example](https://stackify.com/dependency-inversion-principle/)

### Un design pattern

Des développeurs ont déjà résolu le problème de l'inversion de dépendances et ont mis au point un design pattern permettant d'implémenter le principe d'inversion de dépendances.
 
Le design pattern de l'Adapteur permet de transformer un objet d'une classe et le transforme pour qu'il soit utilisable par une autre classe implémentant une interface précise. C'est une méthode qui permet de faire communiquer les objets entre eux sans qu'ils connaissent les détails de l'implémentation de chacun. Ce design pattern est donc un moyen de mettre en place le principe d'inversion de dépendance puisqu'il permet un **couplage faible** entre une classe et une autre, grâce à l'usage d'une interface et d'une méthode de transformation.

Les ressources suivantes te permettront d'avoir un introduction sur le sujet des design patterns et la connaissance du design pattern de l'Adapteur.

#### Ressources

* [Les design patterns](https://sourcemaking.com/design_patterns)
* [L'Adapteur (description)](https://sourcemaking.com/design_patterns/adapter)
* [L'Adapteur (C#)](https://refactoring.guru/design-patterns/adapter/csharp/example)

### Challenge

Corrige la violation du DIP dans le code suivant:
```C#
public class Program
{
    public static void Main()
    {
        Terminal terminal = new Terminal();
        while (!terminal.Exited)
        {
            Command command = terminal.PromptCommand();
            terminal.ExecuteCommand(command);
        }
    }
}

public class Terminal
{
    public bool Exited { get; set; }
    private string _promptString;

    public Terminal()
    {
        _promptString = String.Format("{0}$ ", System.Security.Principal.WindowsIdentity.GetCurrent().Name);
        Exited = false;
    }

    public Command PromptCommand()
    {
        string commandLine = Prompt();
        return new Command(commandLine);
    }

    public string Prompt()
    {
        Console.Write(_promptString);
        string userCommand = Console.ReadLine();
        return userCommand;
    }

    public void ExecuteCommand(Command command)
    {
        try
        {
            command.Launch();
            if (command.Output.Length > 0)
            {
                Console.WriteLine(command.Output);
            }
        }
        catch (InvalidOperationException exception)
        {
            Console.Error.WriteLine("{0}: path not found", command);
        }
    }
}

public class Command
{
    public string Executable { get; private set; }
    public string[] Arguments { get; private set; }
    public string Output { get; private set; }
    public Command(string line)
    {
        string[] splittedLine = line.Split(' ');
        Executable = splittedLine[0];
        Arguments = splittedLine.Skip(1).ToArray();
        Output = "";
    }

    public override string ToString()
    {
        if (ExecutableFullPath is null)
        {
            return Executable;
        }
        else
        {
            return ExecutableFullPath;
        }
    }

    public void Launch()
    {
        string commandOutput;
        if (Executable.Length == 0)
        {
            return;
        }
        using (Process process = new Process())
        {
            process.StartInfo.FileName = ExecutableFullPath;
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.Arguments = String.Join(' ', Arguments);
            process.Start();

            commandOutput = process.StandardOutput.ReadToEnd();
            process.WaitForExit();
        }
        Output = commandOutput;
    }

    public string ExecutableFullPath
    {
        get
        {
            string executableWithExtension;
            if (Executable.EndsWith(".exe"))
            {
                executableWithExtension = Executable;
            }
            else
            {
                executableWithExtension = Executable + ".exe";
            }
            if (File.Exists(executableWithExtension))
            {
                return Path.GetFullPath(Executable);
            }

            string values = Environment.GetEnvironmentVariable("PATH");
            foreach (var path in values.Split(Path.PathSeparator))
            {
                string fullPath = Path.Combine(path, executableWithExtension);
                if (File.Exists(fullPath))
                {
                    return fullPath;
                }
            }
            return null;
        }
    }
}
```

#### Critères de validation

* La solution .sln, le fichier .csproj et les fichiers.cs uniquement sont hébergés sur un dépôt GitHub
* La classe Terminal dépend d'abstractions des commandes, et non pas de son implémentation
* Chaque classe est dans un fichier à part