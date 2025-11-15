using DustInTheWind.JFileDb.New;
using DustInTheWind.JFileDb.Tests.Helpers;

namespace DustInTheWind.JFileDb.Tests.StorageCrawlerTests;

public class AutomaticParentAssignmentTests
{
    [Fact]
    public void Open_WhenCrawlingDocuments_ParentIsSetAutomaticallyByCollection()
    {
        // Arrange
        using TemporaryDatabase temporaryDatabase = new TemporaryDatabase();
        temporaryDatabase.CreateMainDocument("issuer");
        temporaryDatabase.CreateChildDocument("emission");
        temporaryDatabase.CreateChildDocument("coin");

        StorageCrawler storageCrawler = new StorageCrawler(temporaryDatabase.RootPath);

        // Act
        storageCrawler.Open();

        // Assert
        Assert.Single(storageCrawler.Items);
        
        DocumentMetadata issuer = storageCrawler.Items[0];
        Assert.Equal("issuer", issuer.TypeId);
        Assert.Null(issuer.Parent); // Root document has no parent
        
        Assert.Equal(2, issuer.Children.Count);
        
        DocumentMetadata emission = issuer.Children.FirstOrDefault(x => x.TypeId == "emission");
        Assert.NotNull(emission);
        Assert.Equal(issuer, emission.Parent); // Parent set automatically by collection
        
        DocumentMetadata coin = issuer.Children.FirstOrDefault(x => x.TypeId == "coin");
        Assert.NotNull(coin);
        Assert.Equal(issuer, coin.Parent); // Parent set automatically by collection
    }
    
    [Fact]
    public void Open_WhenCrawlingNestedDocuments_ParentChainIsSetAutomaticallyByCollections()
    {
        // Arrange
        using TemporaryDatabase temporaryDatabase = new TemporaryDatabase();
        temporaryDatabase.CreateMainDocument("issuer");
        temporaryDatabase.CreateMainDocument("emission", "romania");
        temporaryDatabase.CreateChildDocument("banknote", "romania");

        StorageCrawler storageCrawler = new StorageCrawler(temporaryDatabase.RootPath);

        // Act
        storageCrawler.Open();

        // Assert
        DocumentMetadata issuer = storageCrawler.Items[0];
        DocumentMetadata emission = issuer.Children[0];
        DocumentMetadata banknote = emission.Children[0];

        // Verify the entire parent chain was set automatically
        Assert.Null(issuer.Parent);
        Assert.Equal(issuer, emission.Parent);
        Assert.Equal(emission, banknote.Parent);
    }
}