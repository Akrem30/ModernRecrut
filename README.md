# ModernRecrut
ModernRecrut est une petite application web (réalisée dans le cadre de ma formation en développement d'applications sécuritaires) qui permet aux entreprises de publier des offres d'emploi et de gérer les candidatures. L'application est développée en utilisant une architecture de microservices, qui offre une grande flexibilité et une évolutivité accrue.

## Microservices
Les microservices de l'application sont les suivants :
- Une API pour gérer les offres d'emploi : Cette API permet de créer, lire, mettre à jour et supprimer des offres d'emploi. Elle est développée en utilisant une architecture propre et offre une interface RESTful pour interagir avec les offres d'emploi.
- Une API pour gérer les postulations et les notes : Cette API permet de gérer les postulations des candidats et les notes attribuées aux candidatures. Elle offre des fonctionnalités pour soumettre des postulations, afficher les postulations soumises, et attribuer et afficher les notes des candidatures.
- Une API pour la gestion des documents : Cette API permet aux candidats de télécharger des fichiers accompagnant leurs candidatures. Elle offre des fonctionnalités pour télécharger, afficher et supprimer des fichiers.
- Une API pour la gestion des favoris : Cette API permet aux candidats de marquer des offres d'emploi comme favoris et de les afficher ultérieurement. Elle offre des fonctionnalités pour ajouter, afficher et supprimer des offres favorites.
## MVC
En plus de ces microservices, une application MVC est développée pour interagir avec les différentes API.
L'authentification et la gestion des rôles sont implémentées au niveau de l'application MVC pour assurer la sécurité et la confidentialité des données.


Dans l'ensemble, cette architecture offre une grande flexibilité et une évolutivité accrue, ce qui permet à l'application de s'adapter aux besoins changeants des entreprises et des candidats. De plus, l'utilisation de microservices permet de réduire la complexité et d'améliorer la maintenabilité de l'application.
