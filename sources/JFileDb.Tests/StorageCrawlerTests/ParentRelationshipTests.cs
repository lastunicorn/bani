using DustInTheWind.JFileDb.New;
using DustInTheWind.JFileDb.Tests.Helpers;

namespace DustInTheWind.JFileDb.Tests.StorageCrawlerTests;

public class ParentRelationshipTests : IDisposable
{
    private readonly TemporaryDatabase temporaryDatabase;

    public ParentRelationshipTests()
    {
        temporaryDatabase = new TemporaryDatabase();
    }

    public void Dispose()
    {
        temporaryDatabase?.Dispose();
    }

    [Fact]
    public void Open_RootLevelDocument_HasNoParent()
    {
        // Arrange
        temporaryDatabase.CreateMainDocument("issuer");
        StorageCrawler crawler = new(temporaryDatabase.RootPath);

        // Act
        crawler.Open();
        List<DocumentMetadata> result = crawler.Items;

        // Assert
        Assert.Single(result);
        Assert.Null(result[0].Parent);
        Assert.Equal("issuer", result[0].TypeId);
    }

    [Fact]
    public void Open_ChildDocument_HasCorrectParent()
    {
        // Arrange
        temporaryDatabase.CreateMainDocument("issuer");
        temporaryDatabase.CreateMainDocument("emission", "romania");
        StorageCrawler crawler = new(temporaryDatabase.RootPath);

        // Act
        crawler.Open();
        List<DocumentMetadata> result = crawler.Items;

        // Assert
        Assert.Single(result);
        DocumentMetadata issuer = result[0];
        Assert.Null(issuer.Parent);
        
        Assert.Single(issuer.Children);
        DocumentMetadata emission = issuer.Children[0];
        Assert.Same(issuer, emission.Parent);
        Assert.Equal("emission", emission.TypeId);
    }

    [Fact]
    public void Open_DeepHierarchy_MaintainsCorrectParentChain()
    {
        // Arrange
        temporaryDatabase.CreateMainDocument("issuer");
        temporaryDatabase.CreateMainDocument("emission", "romania");
        temporaryDatabase.CreateMainDocument("coin", "romania/al patrulea leu");
        temporaryDatabase.CreateMainDocument("banknote", "romania/al patrulea leu/series1");

        StorageCrawler crawler = new(temporaryDatabase.RootPath);

        // Act
        crawler.Open();
        List<DocumentMetadata> result = crawler.Items;

        // Assert
        Assert.Single(result);
        
        // Level 1: Issuer (root)
        DocumentMetadata issuer = result[0];
        Assert.Null(issuer.Parent);
        Assert.Equal("issuer", issuer.TypeId);
        Assert.Single(issuer.Children);

        // Level 2: Emission
        DocumentMetadata emission = issuer.Children[0];
        Assert.Same(issuer, emission.Parent);
        Assert.Equal("emission", emission.TypeId);
        Assert.Single(emission.Children);

        // Level 3: Coin 
        DocumentMetadata coin = emission.Children[0];
        Assert.Same(emission, coin.Parent);
        Assert.Equal("coin", coin.TypeId);
        Assert.Single(coin.Children);

        // Level 4: Banknote under coin
        DocumentMetadata banknote = coin.Children[0];
        Assert.Same(coin, banknote.Parent);
        Assert.Equal("banknote", banknote.TypeId);
        Assert.Empty(banknote.Children);
    }

    [Fact]
    public void Open_SiblingDirectoriesWithDocuments_MaintainsCorrectParentChain()
    {
        // Arrange
        temporaryDatabase.CreateMainDocument("issuer");
        temporaryDatabase.CreateMainDocument("emission", "romania");
        temporaryDatabase.CreateMainDocument("coin", "romania/series1");
        temporaryDatabase.CreateMainDocument("banknote", "romania/series2");

        StorageCrawler crawler = new(temporaryDatabase.RootPath);

        // Act
        crawler.Open();
        List<DocumentMetadata> result = crawler.Items;

        // Assert
        Assert.Single(result);
        
        // Level 1: Issuer (root)
        DocumentMetadata issuer = result[0];
        Assert.Null(issuer.Parent);
        Assert.Equal("issuer", issuer.TypeId);
        Assert.Single(issuer.Children);

        // Level 2: Emission
        DocumentMetadata emission = issuer.Children[0];
        Assert.Same(issuer, emission.Parent);
        Assert.Equal("emission", emission.TypeId);
        Assert.Equal(2, emission.Children.Count);

        // Level 3: Coin and Banknote (sibling directories)
        DocumentMetadata coin = emission.Children.First(x => x.TypeId == "coin");
        DocumentMetadata banknote = emission.Children.First(x => x.TypeId == "banknote");
        
        Assert.Same(emission, coin.Parent);
        Assert.Same(emission, banknote.Parent);
        Assert.Empty(coin.Children);
        Assert.Empty(banknote.Children);
    }

    [Fact]
    public void Open_LocalChildDocument_HasCorrectParent()
    {
        // Arrange
        temporaryDatabase.CreateMainDocument("issuer");
        temporaryDatabase.CreateChildDocument("emission");
        StorageCrawler crawler = new(temporaryDatabase.RootPath);

        // Act
        crawler.Open();
        List<DocumentMetadata> result = crawler.Items;

        // Assert
        Assert.Single(result);
        DocumentMetadata issuer = result[0];
        Assert.Null(issuer.Parent);
        Assert.Single(issuer.Children);

        DocumentMetadata emission = issuer.Children[0];
        Assert.Same(issuer, emission.Parent);
        Assert.Equal("emission", emission.TypeId);
    }

    [Fact]
    public void Open_MultipleChildrenAtSameLevel_AllHaveSameParent()
    {
        // Arrange
        temporaryDatabase.CreateMainDocument("issuer");
        temporaryDatabase.CreateMainDocument("emission", "romania");
        temporaryDatabase.CreateChildDocument("coin", "romania");
        temporaryDatabase.CreateChildDocument("banknote", "romania");

        StorageCrawler crawler = new(temporaryDatabase.RootPath);

        // Act
        crawler.Open();
        List<DocumentMetadata> result = crawler.Items;

        // Assert
        Assert.Single(result);
        DocumentMetadata issuer = result[0];
        Assert.Single(issuer.Children);

        DocumentMetadata emission = issuer.Children[0];
        Assert.Equal(2, emission.Children.Count);

        foreach (DocumentMetadata child in emission.Children)
        {
            Assert.Same(emission, child.Parent);
        }
    }

    [Fact]
    public void Open_OrphanedChildDocuments_AttachedToCorrectParent()
    {
        // Arrange
        temporaryDatabase.CreateMainDocument("issuer");
        temporaryDatabase.CreateMainDocument("emission", "romania");
        temporaryDatabase.CreateChildDocument("coin", "romania/al patrulea leu");

        StorageCrawler crawler = new(temporaryDatabase.RootPath);

        // Act
        crawler.Open();
        List<DocumentMetadata> result = crawler.Items;

        // Assert
        Assert.Single(result);
        DocumentMetadata issuer = result[0];
        DocumentMetadata emission = issuer.Children[0];
        
        Assert.Single(emission.Children);
        DocumentMetadata coin = emission.Children[0];
        Assert.Same(emission, coin.Parent);
        Assert.Equal("coin", coin.TypeId);
    }

    [Fact]
    public void Open_StandaloneChildDocument_HasNoParent()
    {
        // Arrange
        temporaryDatabase.CreateChildDocument("emission");
        StorageCrawler crawler = new(temporaryDatabase.RootPath);

        // Act
        crawler.Open();
        List<DocumentMetadata> result = crawler.Items;

        // Assert
        Assert.Single(result);
        DocumentMetadata emission = result[0];
        Assert.Null(emission.Parent);
        Assert.Equal("emission", emission.TypeId);
    }
}