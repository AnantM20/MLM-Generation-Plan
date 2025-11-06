-- Script to update Admin user to have referrals
-- Run this if database already has data

USE MLMDb;
GO

-- Update REG1001 to be sponsored by Admin (REG1000)
UPDATE Users 
SET SponsorId = 'REG1000' 
WHERE UserId = 'REG1001' AND SponsorId IS NULL;
GO

-- Add additional direct referrals for Admin if they don't exist
IF NOT EXISTS (SELECT * FROM Users WHERE UserId = 'REG1016')
BEGIN
    INSERT INTO Users (FullName, Email, MobileNumber, UserId, Password, SponsorId, IsActive, IsAdmin, RegistrationDate)
    VALUES ('Sarah Connor', 'sarah.connor@example.com', '9876543225', 'REG1016', 'VGVzdEAxMjM=', 'REG1000', 1, 0, DATEADD(DAY, -16, GETDATE()));
END
GO

IF NOT EXISTS (SELECT * FROM Users WHERE UserId = 'REG1017')
BEGIN
    INSERT INTO Users (FullName, Email, MobileNumber, UserId, Password, SponsorId, IsActive, IsAdmin, RegistrationDate)
    VALUES ('Mike Tyson', 'mike.tyson@example.com', '9876543226', 'REG1017', 'VGVzdEAxMjM=', 'REG1000', 1, 0, DATEADD(DAY, -17, GETDATE()));
END
GO

PRINT 'Admin referrals updated successfully!';
GO

