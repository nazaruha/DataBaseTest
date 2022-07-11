SELECT dbo.tblUsers.Id, dbo.tblUsers.Name, dbo.tblRegions.Name AS Region, dbo.tblCities.Name AS City, dbo.tblUserAddresses.Street, dbo.tblUserAddresses.HouseNumber
FROM   dbo.tblCities INNER JOIN
             dbo.tblRegions ON dbo.tblCities.RegionId = dbo.tblRegions.Id INNER JOIN
             dbo.tblUserAddresses ON dbo.tblCities.Id = dbo.tblUserAddresses.CityId INNER JOIN
             dbo.tblUsers ON dbo.tblUserAddresses.UserId = dbo.tblUsers.Id