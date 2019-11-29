CREATE TABLE [dbo].[Game] (
    [Id]                INT            NOT NULL IDENTITY,
    [BlackPlayerAvatar] VARCHAR (255)  NOT NULL,
    [WhitePlayerAvatar] VARCHAR (255)  NULL,
    [Description]       VARCHAR (1000) NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    FOREIGN KEY ([BlackPlayerAvatar]) REFERENCES [dbo].[Player] ([Avatar])
);

