USE [CBHIS]
GO

/****** Object:  Table [dbo].[WyNhRegister]    Script Date: 12/03/2015 23:19:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[WyNhRegister](
	[NhRegID] [uniqueidentifier] NOT NULL,
	[OrganCode] [varchar](6) NOT NULL,
	[AccountYear] [varchar](4) NOT NULL,
	[CoopMedCode] [varchar](18) NOT NULL,
	[ExpressionID] [varchar](4) NOT NULL,
	[PatientName] [varchar](10) NOT NULL,
	[AiIDNo] [int] NOT NULL,
	[IllCode] [varchar](10) NOT NULL,
	[IllName] [varchar](40) NOT NULL,
	[InDate] [varchar](20) NOT NULL,
	[Adke] [varchar](12) NULL,
	[AdLimitDef] [varchar](10) NOT NULL,
	[DoctorName] [varchar](12) NULL,
	[PatientID] [varchar](50) NULL,
	[DiagNo] [varchar](2) NULL,
	[ExpenseKind] [varchar](2) NULL,
	[LimitIllCode] [varchar](4) NULL,
	[IsFail] [tinyint] NOT NULL,
	[Zyh] [int] NOT NULL,
	[AreaCode] [varchar](12) NOT NULL,
	[TurnID] [int] NULL,
	[FunHrStr] [varchar](200) NULL,
 CONSTRAINT [PK_WyNhRegister] PRIMARY KEY CLUSTERED 
(
	[NhRegID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

