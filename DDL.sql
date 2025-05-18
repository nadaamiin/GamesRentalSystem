/*==============================================================*/
/* DBMS name:      Microsoft SQL Server 2005                    */
/* Created on:     5/15/2025 7:11:33 PM                         */
/*==============================================================*/


if exists (select 1
   from sys.sysreferences r join sys.sysobjects o on (o.id = r.constid and o.type = 'F')
   where r.fkeyid = object_id('ADMINPHONE') and o.name = 'FK_ADMINPHO_OWNS_ADMIN')
alter table ADMINPHONE
   drop constraint FK_ADMINPHO_OWNS_ADMIN
go

if exists (select 1
   from sys.sysreferences r join sys.sysobjects o on (o.id = r.constid and o.type = 'F')
   where r.fkeyid = object_id('CARD') and o.name = 'FK_CARD_HAVE_USER')
alter table CARD
   drop constraint FK_CARD_HAVE_USER
go

if exists (select 1
   from sys.sysreferences r join sys.sysobjects o on (o.id = r.constid and o.type = 'F')
   where r.fkeyid = object_id('GAME') and o.name = 'FK_GAME_ADDEDBY_ADMIN')
alter table GAME
   drop constraint FK_GAME_ADDEDBY_ADMIN
go

if exists (select 1
   from sys.sysreferences r join sys.sysobjects o on (o.id = r.constid and o.type = 'F')
   where r.fkeyid = object_id('GAME') and o.name = 'FK_GAME_DEVELOP_VENDOR')
alter table GAME
   drop constraint FK_GAME_DEVELOP_VENDOR
go

if exists (select 1
   from sys.sysreferences r join sys.sysobjects o on (o.id = r.constid and o.type = 'F')
   where r.fkeyid = object_id('GAMECATEGORY') and o.name = 'FK_GAMECATE_CATEGORIZ_GAME')
alter table GAMECATEGORY
   drop constraint FK_GAMECATE_CATEGORIZ_GAME
go

if exists (select 1
   from sys.sysreferences r join sys.sysobjects o on (o.id = r.constid and o.type = 'F')
   where r.fkeyid = object_id('RENTED_BY') and o.name = 'FK_RENTED_B_RELATIONS_GAME')
alter table RENTED_BY
   drop constraint FK_RENTED_B_RELATIONS_GAME
go

if exists (select 1
   from sys.sysreferences r join sys.sysobjects o on (o.id = r.constid and o.type = 'F')
   where r.fkeyid = object_id('RENTED_BY') and o.name = 'FK_RENTED_B_RELATIONS_USER')
alter table RENTED_BY
   drop constraint FK_RENTED_B_RELATIONS_USER
go

if exists (select 1
   from sys.sysreferences r join sys.sysobjects o on (o.id = r.constid and o.type = 'F')
   where r.fkeyid = object_id('USERPHONE') and o.name = 'FK_USERPHON_OBTAINS_USER')
alter table USERPHONE
   drop constraint FK_USERPHON_OBTAINS_USER
go

if exists (select 1
   from sys.sysreferences r join sys.sysobjects o on (o.id = r.constid and o.type = 'F')
   where r.fkeyid = object_id('VENDORPHONE') and o.name = 'FK_VENDORPH_HAS_VENDOR')
alter table VENDORPHONE
   drop constraint FK_VENDORPH_HAS_VENDOR
go

if exists (select 1
            from  sysobjects
           where  id = object_id('ADMIN')
            and   type = 'U')
   drop table ADMIN
go

if exists (select 1
            from  sysindexes
           where  id    = object_id('ADMINPHONE')
            and   name  = 'OWNS_FK'
            and   indid > 0
            and   indid < 255)
   drop index ADMINPHONE.OWNS_FK
go

if exists (select 1
            from  sysobjects
           where  id = object_id('ADMINPHONE')
            and   type = 'U')
   drop table ADMINPHONE
go

if exists (select 1
            from  sysindexes
           where  id    = object_id('CARD')
            and   name  = 'HAVE_FK'
            and   indid > 0
            and   indid < 255)
   drop index CARD.HAVE_FK
go

if exists (select 1
            from  sysobjects
           where  id = object_id('CARD')
            and   type = 'U')
   drop table CARD
go

if exists (select 1
            from  sysindexes
           where  id    = object_id('GAME')
            and   name  = 'ADDEDBY_FK'
            and   indid > 0
            and   indid < 255)
   drop index GAME.ADDEDBY_FK
go

if exists (select 1
            from  sysindexes
           where  id    = object_id('GAME')
            and   name  = 'DEVELOP_FK'
            and   indid > 0
            and   indid < 255)
   drop index GAME.DEVELOP_FK
go

if exists (select 1
            from  sysobjects
           where  id = object_id('GAME')
            and   type = 'U')
   drop table GAME
go

if exists (select 1
            from  sysindexes
           where  id    = object_id('GAMECATEGORY')
            and   name  = 'CATEGORIZEDAS_FK'
            and   indid > 0
            and   indid < 255)
   drop index GAMECATEGORY.CATEGORIZEDAS_FK
go

if exists (select 1
            from  sysobjects
           where  id = object_id('GAMECATEGORY')
            and   type = 'U')
   drop table GAMECATEGORY
go

if exists (select 1
            from  sysindexes
           where  id    = object_id('RENTED_BY')
            and   name  = 'RELATIONSHIP_11_FK'
            and   indid > 0
            and   indid < 255)
   drop index RENTED_BY.RELATIONSHIP_11_FK
go

if exists (select 1
            from  sysindexes
           where  id    = object_id('RENTED_BY')
            and   name  = 'RELATIONSHIP_10_FK'
            and   indid > 0
            and   indid < 255)
   drop index RENTED_BY.RELATIONSHIP_10_FK
go

if exists (select 1
            from  sysobjects
           where  id = object_id('RENTED_BY')
            and   type = 'U')
   drop table RENTED_BY
go

if exists (select 1
            from  sysobjects
           where  id = object_id('"USER"')
            and   type = 'U')
   drop table "USER"
go

if exists (select 1
            from  sysindexes
           where  id    = object_id('USERPHONE')
            and   name  = 'OBTAINS_FK'
            and   indid > 0
            and   indid < 255)
   drop index USERPHONE.OBTAINS_FK
go

if exists (select 1
            from  sysobjects
           where  id = object_id('USERPHONE')
            and   type = 'U')
   drop table USERPHONE
go

if exists (select 1
            from  sysobjects
           where  id = object_id('VENDOR')
            and   type = 'U')
   drop table VENDOR
go

if exists (select 1
            from  sysindexes
           where  id    = object_id('VENDORPHONE')
            and   name  = 'HAS_FK'
            and   indid > 0
            and   indid < 255)
   drop index VENDORPHONE.HAS_FK
go

if exists (select 1
            from  sysobjects
           where  id = object_id('VENDORPHONE')
            and   type = 'U')
   drop table VENDORPHONE
go

/*==============================================================*/
/* Table: ADMIN                                                 */
/*==============================================================*/
create table ADMIN (
   ADDRESS              varchar(100)         null,
   USERNAME             varchar(50)          not null,
   PASSWORD             varchar(50)          not null,
   ADMINID              int                  not null,
   EMAIL                varchar(100)         null,
   constraint PK_ADMIN primary key nonclustered (ADMINID)
)
go

/*==============================================================*/
/* Table: ADMINPHONE                                            */
/*==============================================================*/
create table ADMINPHONE (
   ADMINID              int                  not null,
   APHONE               varchar(50)          not null,
   constraint PK_ADMINPHONE primary key nonclustered (ADMINID, APHONE)
)
go

/*==============================================================*/
/* Index: OWNS_FK                                               */
/*==============================================================*/
create index OWNS_FK on ADMINPHONE (
ADMINID ASC
)
go

/*==============================================================*/
/* Table: CARD                                                  */
/*==============================================================*/
create table CARD (
   HOLDERNAME           varchar(30)          null,
   CARDTYPE             varchar(30)          null,
   CARDID               int                  not null,
   USERID               int                  not null,
   BANKNAME             varchar(50)          null,
   constraint PK_CARD primary key nonclustered (CARDID)
)
go

/*==============================================================*/
/* Index: HAVE_FK                                               */
/*==============================================================*/
create index HAVE_FK on CARD (
USERID ASC
)
go

/*==============================================================*/
/* Table: GAME                                                  */
/*==============================================================*/
create table GAME (
   VENDORID             int                  not null,
   GAMEID               int                  not null,
   ADMINID              int                  null,
   DESCRIPTION          varchar(50)          null,
   QUANTITY             int                  null,
   RELEASEYEAR          int                  null,
   NAME                 varchar(50)          not null,
   constraint PK_GAME primary key nonclustered (VENDORID, GAMEID)
)
go

/*==============================================================*/
/* Index: DEVELOP_FK                                            */
/*==============================================================*/
create index DEVELOP_FK on GAME (
VENDORID ASC
)
go

/*==============================================================*/
/* Index: ADDEDBY_FK                                            */
/*==============================================================*/
create index ADDEDBY_FK on GAME (
ADMINID ASC
)
go

/*==============================================================*/
/* Table: GAMECATEGORY                                          */
/*==============================================================*/
create table GAMECATEGORY (
   VENDORID             int                  not null,
   GAMEID               int                  not null,
   CATEOGRY             varchar(30)          not null,
   constraint PK_GAMECATEGORY primary key nonclustered (VENDORID, GAMEID, CATEOGRY)
)
go

/*==============================================================*/
/* Index: CATEGORIZEDAS_FK                                      */
/*==============================================================*/
create index CATEGORIZEDAS_FK on GAMECATEGORY (
VENDORID ASC,
GAMEID ASC
)
go

/*==============================================================*/
/* Table: RENTED_BY                                             */
/*==============================================================*/
create table RENTED_BY (
   VENDORID             int                  not null,
   GAMEID               int                  not null,
   USERID               int                  not null,
   DATE                 datetime             not null,
   FEES                 int                  null,
   constraint PK_RENTED_BY primary key nonclustered (VENDORID, GAMEID, USERID, DATE)
)
go

/*==============================================================*/
/* Index: RELATIONSHIP_10_FK                                    */
/*==============================================================*/
create index RELATIONSHIP_10_FK on RENTED_BY (
VENDORID ASC,
GAMEID ASC
)
go

/*==============================================================*/
/* Index: RELATIONSHIP_11_FK                                    */
/*==============================================================*/
create index RELATIONSHIP_11_FK on RENTED_BY (
USERID ASC
)
go

/*==============================================================*/
/* Table: "USER"                                                */
/*==============================================================*/
create table "USER" (
   ADDRESS              varchar(100)         null,
   USERNAME             varchar(50)          not null,
   PASSWORD             varchar(50)          not null,
   USERID               int                  not null,
   EMAIL                varchar(100)         null,
   constraint PK_USER primary key nonclustered (USERID)
)
go

/*==============================================================*/
/* Table: USERPHONE                                             */
/*==============================================================*/
create table USERPHONE (
   USERID               int                  not null,
   UPHONE               varchar(50)          not null,
   constraint PK_USERPHONE primary key nonclustered (USERID, UPHONE)
)
go

/*==============================================================*/
/* Index: OBTAINS_FK                                            */
/*==============================================================*/
create index OBTAINS_FK on USERPHONE (
USERID ASC
)
go

/*==============================================================*/
/* Table: VENDOR                                                */
/*==============================================================*/
create table VENDOR (
   NAME                 varchar(50)          not null,
   SPECIALIZATION       varchar(50)          null,
   VENDORID             int                  not null,
   EMAIL                varchar(100)         null,
   constraint PK_VENDOR primary key nonclustered (VENDORID)
)
go

/*==============================================================*/
/* Table: VENDORPHONE                                           */
/*==============================================================*/
create table VENDORPHONE (
   VENDORID             int                  not null,
   VPHONE               varchar(50)          not null,
   constraint PK_VENDORPHONE primary key nonclustered (VENDORID, VPHONE)
)
go

/*==============================================================*/
/* Index: HAS_FK                                                */
/*==============================================================*/
create index HAS_FK on VENDORPHONE (
VENDORID ASC
)
go

alter table ADMINPHONE
   add constraint FK_ADMINPHO_OWNS_ADMIN foreign key (ADMINID)
      references ADMIN (ADMINID)
go

alter table CARD
   add constraint FK_CARD_HAVE_USER foreign key (USERID)
      references "USER" (USERID)
go

alter table GAME
   add constraint FK_GAME_ADDEDBY_ADMIN foreign key (ADMINID)
      references ADMIN (ADMINID)
go

alter table GAME
   add constraint FK_GAME_DEVELOP_VENDOR foreign key (VENDORID)
      references VENDOR (VENDORID)
go

alter table GAMECATEGORY
   add constraint FK_GAMECATE_CATEGORIZ_GAME foreign key (VENDORID, GAMEID)
      references GAME (VENDORID, GAMEID)
go

alter table RENTED_BY
   add constraint FK_RENTED_B_RELATIONS_GAME foreign key (VENDORID, GAMEID)
      references GAME (VENDORID, GAMEID)
go

alter table RENTED_BY
   add constraint FK_RENTED_B_RELATIONS_USER foreign key (USERID)
      references "USER" (USERID)
go

alter table USERPHONE
   add constraint FK_USERPHON_OBTAINS_USER foreign key (USERID)
      references "USER" (USERID)
go

alter table VENDORPHONE
   add constraint FK_VENDORPH_HAS_VENDOR foreign key (VENDORID)
      references VENDOR (VENDORID)
go

