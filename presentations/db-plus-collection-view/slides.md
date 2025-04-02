---
theme: seriph
background: https://cover.sli.dev
title: DB i Collection View
class: text-center
drawings:
  persist: false
mdc: true
defaults:
  transition: fade
---

# DB i Collection View

---
layout: section
transition: slide-left
---

# MODELS

---
layout: image-right
image: @/snippets/DBApp/DBApp.Android/Resources/drawable/icon_food.png
transition: slide-right
---

## FoodModel
Określa co jest w tabeli jedzenia

<<< @/snippets/DBApp/DBApp/Models/Food.cs#snippet csharp {monaco}

---
layout: image-left
image: @/snippets/DBApp/DBApp.Android/Resources/drawable/icon_client.png
transition: slide-left
---

## ClientModel
Określa co jest w tabeli klientów

<<< @/snippets/DBApp/DBApp/Models/Client.cs#snippet csharp {monaco}

---
layout: image-right
image: @/snippets/DBApp/DBApp.Android/Resources/drawable/icon_favourite.png
--- 

## FavouriteFoodModel
Określa co jest w tabeli relacji jedzenie-klient

<<< @/snippets/DBApp/DBApp/Models/FavouriteFood.cs#snippet csharp {monaco}

---
layout: section
transition: slide-left
---

# SERVICES

---
transition: slide-right
---

## DB
Połączenie z bazą danych

<<< @/snippets/DBApp/DBApp/Services/DB.cs#snippet csharp {monaco}

---
layout: image-left
image: https://baconmockup.com/1000/2000
transition: slide-left
fonts:
    sans: Robot
    serif: Robot Slab
    mono: Fira Code
---

## FoodService
Zarządzanie tabelą jedzenia ze strony backend

````md magic-move {class:'w-100'}
<<< @/snippets/DBApp/DBApp/Services/FoodService.cs#constructor csharp
<<< @/snippets/DBApp/DBApp/Services/FoodService.cs#get csharp
<<< @/snippets/DBApp/DBApp/Services/FoodService.cs#save csharp
<<< @/snippets/DBApp/DBApp/Services/FoodService.cs#update csharp
<<< @/snippets/DBApp/DBApp/Services/FoodService.cs#delete csharp
````

---
layout: image-right
image: https://baconmockup.com/1001/2002
transition: slide-right
---

## ClientService
Zarządzanie tabelą klienta ze strony backend

````md magic-move {class:'w-105'}
<<< @/snippets/DBApp/DBApp/Services/ClientService.cs#constructor csharp
<<< @/snippets/DBApp/DBApp/Services/ClientService.cs#get csharp
<<< @/snippets/DBApp/DBApp/Services/ClientService.cs#save csharp
<<< @/snippets/DBApp/DBApp/Services/ClientService.cs#update csharp
<<< @/snippets/DBApp/DBApp/Services/ClientService.cs#delete csharp
````

---
layout: intro
---

## FavouriteFood
Zarządzanie tabelą relacji jedzenie-klient ze strony backend

````md magic-move
<<< @/snippets/DBApp/DBApp/Services/FavouriteFoodService.cs#constructor csharp
<<< @/snippets/DBApp/DBApp/Services/FavouriteFoodService.cs#get csharp
<<< @/snippets/DBApp/DBApp/Services/FavouriteFoodService.cs#save csharp
<<< @/snippets/DBApp/DBApp/Services/FavouriteFoodService.cs#update csharp
<<< @/snippets/DBApp/DBApp/Services/FavouriteFoodService.cs#delete csharp
````

---
layout: section
transition: slide-up
---

# VIEWMODELS

---
transition: slide-up
---

## FoodViewModel
Pobiera dane z FoodPage za pomocą bindingu i przekazuje do modelu Food

````md magic-move
<<< @/snippets/DBApp/DBApp/ViewModels/FoodViewModel.cs#onPropertyChangedInt csharp
<<< @/snippets/DBApp/DBApp/ViewModels/FoodViewModel.cs#inStockInt csharp
<<< @/snippets/DBApp/DBApp/ViewModels/FoodViewModel.cs#foodTypeInt csharp
<<< @/snippets/DBApp/DBApp/ViewModels/FoodViewModel.cs#constructor csharp
<<< @/snippets/DBApp/DBApp/ViewModels/FoodViewModel.cs#loadFood csharp
<<< @/snippets/DBApp/DBApp/ViewModels/FoodViewModel.cs#addFood csharp
<<< @/snippets/DBApp/DBApp/ViewModels/FoodViewModel.cs#onPropertyChanged csharp
````

---
transition: slide-up
---

## ClientViewModel
Pobiera dane z ClientPage za pomocą bindingu i przekazuje do modelu Client

````md magic-move
<<< @/snippets/DBApp/DBApp/ViewModels/ClientViewModel.cs#clientName csharp
<<< @/snippets/DBApp/DBApp/ViewModels/ClientViewModel.cs#clientSurname csharp
<<< @/snippets/DBApp/DBApp/ViewModels/ClientViewModel.cs#constructor csharp
<<< @/snippets/DBApp/DBApp/ViewModels/ClientViewModel.cs#loadClient csharp
<<< @/snippets/DBApp/DBApp/ViewModels/ClientViewModel.cs#addClient csharp
<<< @/snippets/DBApp/DBApp/ViewModels/ClientViewModel.cs#onPropertyChanged csharp
````

---

## FavouriteFoodViewModel
Pobiera dane z FavouriteFoodPage za pomocą bindingu i przekazuje do modelu FavouriteFood

````md magic-move
<<< @/snippets/DBApp/DBApp/ViewModels/FavourtieFoodViewModel.cs#selectedFood csharp
<<< @/snippets/DBApp/DBApp/ViewModels/FavourtieFoodViewModel.cs#selectedClient csharp
<<< @/snippets/DBApp/DBApp/ViewModels/FavourtieFoodViewModel.cs#constructor csharp
<<< @/snippets/DBApp/DBApp/ViewModels/FavourtieFoodViewModel.cs#loadData csharp
<<< @/snippets/DBApp/DBApp/ViewModels/FavourtieFoodViewModel.cs#loadFood csharp
<<< @/snippets/DBApp/DBApp/ViewModels/FavourtieFoodViewModel.cs#loadClient csharp
<<< @/snippets/DBApp/DBApp/ViewModels/FavourtieFoodViewModel.cs#loadFavFood csharp
<<< @/snippets/DBApp/DBApp/ViewModels/FavourtieFoodViewModel.cs#addFavFood csharp
<<< @/snippets/DBApp/DBApp/ViewModels/FavourtieFoodViewModel.cs#onPropertyChanged csharp
````

---
layout: section
transition: slide-left
---

# VIEWS

---
layout: two-cols-header
transition: slide-right
---

::left::

## FoodPage
Guziki poszczute bindingiem i wyświetlanie danych jedzenia użytkownikowi za pomocą CollectionView

````md magic-move {class: 'w-160'}
<<< @/snippets/DBApp/DBApp/Views/FoodPage.xaml#insert xml
<<< @/snippets/DBApp/DBApp/Views/FoodPage.xaml#view xml
````

::right::

<img src="https://cooligus.s3.eu-central-1.amazonaws.com/db-app/foods-empty.png" alt="food-empty" class="w-50" style="float: right" />

---
layout: two-cols-header
transition: slide-left
---

::left::

<img src="https://cooligus.s3.eu-central-1.amazonaws.com/db-app/clients-empty.png" alt="clients-empty" class="w-50" style="float: left" />

::right::

## ClientPage
Guziki poszczute bindingiem i wyświetlanie danych klientów użytkownikowi za pomocą CollectionView

````md magic-move {class: 'w-160'}
<<< @/snippets/DBApp/DBApp/Views/ClientPage.xaml#insert xml
<<< @/snippets/DBApp/DBApp/Views/ClientPage.xaml#view xml
````

---
layout: two-cols-header
---

::left::

## FavouriteFoodPage
Guziki poszczute bindingiem i wyświetlanie danych relacji klient-jedzenie użytkownikowi za pomocą CollectionView

````md magic-move {class: 'w-160'}
<<< @/snippets/DBApp/DBApp/Views/FavouriteFoodPage.xaml#insert xml
<<< @/snippets/DBApp/DBApp/Views/FavouriteFoodPage.xaml#view xml
````

::right::

<img src="https://cooligus.s3.eu-central-1.amazonaws.com/db-app/favourite-foods-empty.png" alt="fav-empty" class="w-50" style="float: right" />

---
background: https://cover.sli.dev
class: text-center
layout: cover
---

# Dziękuję za uwagę