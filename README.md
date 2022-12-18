# T120B165 Saityno taikomųjų programų projektavimas projektas "Bendrabutis"

## Sistemos aprašymas

Projektas yra skirtas suteikti tiek bendrabučio gyventojams, tiek jo administratoriams patogią bei
lengvai perprantamą sąsają gyvenimo bendrabutyje planavimui.

Veikimo principas – naudotojams bus pateikta grafinė sąsaja, kuri mainysis duomenimis su
aplikacijų programavimo sąsaja (angl. API).

Svečias (darome prielaidą, jog jis yra studentas) norėdamas apsigyvendinti bendrabutyje turi
pateikti prašymą gyventi viename iš kambarių. Svečias taip pat gali matyti laisvų vietų
bendrabučiuose skaičių.
Bendrabučio gyventojas mato informaciją apie savo gyvenamą vietą, gali
pateikti prašymą persikelti į kitą bendrabutį. Administratorius tvirtina prašymus apsigyventi bei taip
pat mato laisvų vietų bendrabučiuose skaičių.
Visi prašymai iki patvirtinimo stovi eilėje pagal pateikimo datą.

API pasiekiamas - https://bendrabutissystem.azurewebsites.net

Applikacija pasiekiama - https://bendrabutiswen.azurewebsites.net

## Aplikacijos wireframes ir atitinkantys langai

PDF failą su wireframes galite rasti [čia](wireframes.pdf) (wireframes.pdf).

## API dokumentacija

PDF failą su API dokumentacija galite rasti [čia](API_dokumentacija.pdf) (API_dokumentacija.pdf).

## Projekto išvados

1. Projektas įgyvendintas sėkmingai. Sukurtos pagrindinės objektų valdymo operacijos, veikianti sistema.
2. Naudojantis Azure Applicantion Service projekto serverinė bei klientinė pusė yra pasikiamos viešai tinkle.
3. Projekto serverinė dalis įgyvendinta pagal pagrindinius REST principus, kiekvienos operacijos rezultatą galima atpažinti pagal užklausos atsako kodą (response code).
4. Projekto klientinė dalis atlikta su React.JS turi responsive design, animacijas bei lengvai palaikomus elementus.
5. Projekto vartotojo sąsaja atitinka nubrėžtus wireframes.
