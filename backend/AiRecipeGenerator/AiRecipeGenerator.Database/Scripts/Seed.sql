-- Populate IngredientCategories
INSERT INTO IngredientCategories (Name, CreatedOn, UpdatedOn) VALUES
('Meat & Poultry', datetime('now'), datetime('now')),
('Fish & Seafood', datetime('now'), datetime('now')),
('Eggs', datetime('now'), datetime('now')),
('Grains & Cereals', datetime('now'), datetime('now')),
('Legumes & Nuts', datetime('now'), datetime('now')),
('Vegetables', datetime('now'), datetime('now')),
('Fruits', datetime('now'), datetime('now')),
('Dairy', datetime('now'), datetime('now')),
('Mushrooms', datetime('now'), datetime('now'));

-- Populate Ingredients
-- Category 1: Meat & Poultry
INSERT INTO Ingredients (Name, CategoryId, IsVisibleOnCard, CreatedOn, UpdatedOn, ImagePath) VALUES
('Chicken Breast', 1, 1, datetime('now'), datetime('now'), NULL),
('Ground Beef', 1, 1, datetime('now'), datetime('now'), NULL),
('Pork Chops', 1, 1, datetime('now'), datetime('now'), NULL),
('Turkey', 1, 1, datetime('now'), datetime('now'), NULL),
('Lamb', 1, 1, datetime('now'), datetime('now'), NULL),
('Bacon', 1, 0, datetime('now'), datetime('now'), NULL),
('Duck', 1, 0, datetime('now'), datetime('now'), NULL),
('Sausage', 1, 0, datetime('now'), datetime('now'), NULL),
('Ham', 1, 0, datetime('now'), datetime('now'), NULL),
('Veal', 1, 0, datetime('now'), datetime('now'), NULL);

-- Category 2: Fish & Seafood
INSERT INTO Ingredients (Name, CategoryId, IsVisibleOnCard, CreatedOn, UpdatedOn, ImagePath) VALUES
('Salmon', 2, 1, datetime('now'), datetime('now'), NULL),
('Cod', 2, 1, datetime('now'), datetime('now'), NULL),
('Tuna', 2, 1, datetime('now'), datetime('now'), NULL),
('Shrimp', 2, 1, datetime('now'), datetime('now'), NULL),
('Crab', 2, 1, datetime('now'), datetime('now'), NULL),
('Anchovies', 2, 0, datetime('now'), datetime('now'), NULL),
('Lobster', 2, 0, datetime('now'), datetime('now'), NULL),
('Mussels', 2, 0, datetime('now'), datetime('now'), NULL),
('Oysters', 2, 0, datetime('now'), datetime('now'), NULL),
('Mackerel', 2, 0, datetime('now'), datetime('now'), NULL);

-- Category 3: Eggs
INSERT INTO Ingredients (Name, CategoryId, IsVisibleOnCard, CreatedOn, UpdatedOn, ImagePath) VALUES
('Chicken Eggs', 3, 1, datetime('now'), datetime('now'), NULL),
('Quail Eggs', 3, 1, datetime('now'), datetime('now'), NULL),
('Duck Eggs', 3, 1, datetime('now'), datetime('now'), NULL),
('Egg Whites', 3, 1, datetime('now'), datetime('now'), NULL),
('Egg Yolks', 3, 1, datetime('now'), datetime('now'), NULL),
('Goose Eggs', 3, 0, datetime('now'), datetime('now'), NULL),
('Ostrich Eggs', 3, 0, datetime('now'), datetime('now'), NULL),
('Emu Eggs', 3, 0, datetime('now'), datetime('now'), NULL),
('Turkey Eggs', 3, 0, datetime('now'), datetime('now'), NULL),
('Pheasant Eggs', 3, 0, datetime('now'), datetime('now'), NULL);

-- Category 4: Grains & Cereals
INSERT INTO Ingredients (Name, CategoryId, IsVisibleOnCard, CreatedOn, UpdatedOn, ImagePath) VALUES
('Rice', 4, 1, datetime('now'), datetime('now'), NULL),
('Wheat', 4, 1, datetime('now'), datetime('now'), NULL),
('Oats', 4, 1, datetime('now'), datetime('now'), NULL),
('Barley', 4, 1, datetime('now'), datetime('now'), NULL),
('Pasta', 4, 1, datetime('now'), datetime('now'), NULL),
('Corn', 4, 0, datetime('now'), datetime('now'), NULL),
('Rye', 4, 0, datetime('now'), datetime('now'), NULL),
('Quinoa', 4, 0, datetime('now'), datetime('now'), NULL),
('Millet', 4, 0, datetime('now'), datetime('now'), NULL),
('Buckwheat', 4, 0, datetime('now'), datetime('now'), NULL);

-- Category 5: Legumes & Nuts
INSERT INTO Ingredients (Name, CategoryId, IsVisibleOnCard, CreatedOn, UpdatedOn, ImagePath) VALUES
('Almonds', 5, 1, datetime('now'), datetime('now'), NULL),
('Peanuts', 5, 1, datetime('now'), datetime('now'), NULL),
('Chickpeas', 5, 1, datetime('now'), datetime('now'), NULL),
('Lentils', 5, 1, datetime('now'), datetime('now'), NULL),
('Black Beans', 5, 1, datetime('now'), datetime('now'), NULL),
('Walnuts', 5, 0, datetime('now'), datetime('now'), NULL),
('Cashews', 5, 0, datetime('now'), datetime('now'), NULL),
('Pistachios', 5, 0, datetime('now'), datetime('now'), NULL),
('Kidney Beans', 5, 0, datetime('now'), datetime('now'), NULL),
('Pecans', 5, 0, datetime('now'), datetime('now'), NULL);

-- Category 6: Vegetables
INSERT INTO Ingredients (Name, CategoryId, IsVisibleOnCard, CreatedOn, UpdatedOn, ImagePath) VALUES
('Tomato', 6, 1, datetime('now'), datetime('now'), NULL),
('Onion', 6, 1, datetime('now'), datetime('now'), NULL),
('Garlic', 6, 1, datetime('now'), datetime('now'), NULL),
('Bell Pepper', 6, 1, datetime('now'), datetime('now'), NULL),
('Broccoli', 6, 1, datetime('now'), datetime('now'), NULL),
('Carrot', 6, 0, datetime('now'), datetime('now'), NULL),
('Potato', 6, 0, datetime('now'), datetime('now'), NULL),
('Spinach', 6, 0, datetime('now'), datetime('now'), NULL),
('Lettuce', 6, 0, datetime('now'), datetime('now'), NULL),
('Cucumber', 6, 0, datetime('now'), datetime('now'), NULL);

-- Category 7: Fruits
INSERT INTO Ingredients (Name, CategoryId, IsVisibleOnCard, CreatedOn, UpdatedOn, ImagePath) VALUES
('Apple', 7, 1, datetime('now'), datetime('now'), NULL),
('Banana', 7, 1, datetime('now'), datetime('now'), NULL),
('Orange', 7, 1, datetime('now'), datetime('now'), NULL),
('Strawberry', 7, 1, datetime('now'), datetime('now'), NULL),
('Blueberry', 7, 1, datetime('now'), datetime('now'), NULL),
('Grape', 7, 0, datetime('now'), datetime('now'), NULL),
('Mango', 7, 0, datetime('now'), datetime('now'), NULL),
('Pineapple', 7, 0, datetime('now'), datetime('now'), NULL),
('Lemon', 7, 0, datetime('now'), datetime('now'), NULL),
('Avocado', 7, 0, datetime('now'), datetime('now'), NULL);

-- Category 8: Dairy
INSERT INTO Ingredients (Name, CategoryId, IsVisibleOnCard, CreatedOn, UpdatedOn, ImagePath) VALUES
('Milk', 8, 1, datetime('now'), datetime('now'), NULL),
('Cheese', 8, 1, datetime('now'), datetime('now'), NULL),
('Butter', 8, 1, datetime('now'), datetime('now'), NULL),
('Yogurt', 8, 1, datetime('now'), datetime('now'), NULL),
('Cream', 8, 1, datetime('now'), datetime('now'), NULL),
('Sour Cream', 8, 0, datetime('now'), datetime('now'), NULL),
('Cottage Cheese', 8, 0, datetime('now'), datetime('now'), NULL),
('Mozzarella', 8, 0, datetime('now'), datetime('now'), NULL),
('Cheddar', 8, 0, datetime('now'), datetime('now'), NULL),
('Feta', 8, 0, datetime('now'), datetime('now'), NULL);

-- Category 9: Mushrooms
INSERT INTO Ingredients (Name, CategoryId, IsVisibleOnCard, CreatedOn, UpdatedOn, ImagePath) VALUES
('Button Mushroom', 9, 1, datetime('now'), datetime('now'), NULL),
('Portobello', 9, 1, datetime('now'), datetime('now'), NULL),
('Shiitake', 9, 1, datetime('now'), datetime('now'), NULL),
('Oyster Mushroom', 9, 1, datetime('now'), datetime('now'), NULL),
('Cremini', 9, 1, datetime('now'), datetime('now'), NULL),
('Chanterelle', 9, 0, datetime('now'), datetime('now'), NULL),
('Porcini', 9, 0, datetime('now'), datetime('now'), NULL),
('Truffle', 9, 0, datetime('now'), datetime('now'), NULL),
('Enoki', 9, 0, datetime('now'), datetime('now'), NULL),
('Morel', 9, 0, datetime('now'), datetime('now'), NULL);

-- Populate ApiKeys
INSERT INTO ApiKeys (Key, CreatedOn) VALUES
('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', datetime('now'));
