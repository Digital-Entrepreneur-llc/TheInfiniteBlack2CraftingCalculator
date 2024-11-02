using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace TheInfiniteBlack2CraftingCalculator
{
    public partial class MainForm : Form
    {
        private Dictionary<string, Recipe> recipes = new Dictionary<string, Recipe>();
        private ResourceManager resourceManager;
        private Button configButton;
        private ComboBox categorySelector;
        private ComboBox factionSelector;
        private Panel contentPanel;
        private ListBox recipeList;
        private Panel recipeDetailsPanel;

        public MainForm()
        {
            InitializeComponent();
            resourceManager = new ResourceManager();
            InitializeRecipes();
        }

        private void InitializeComponent()
        {
            this.Text = "TIB2 Crafting Calculator";
            this.Size = new Size(580, 440);
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = true;
            this.BackColor = Color.FromArgb(240, 240, 240);
            this.Font = new Font("Segoe UI", 9F, FontStyle.Regular);

            // Main container
            TableLayoutPanel mainLayout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 1,
                RowCount = 2,  // Reduced to 2 rows
                Padding = new Padding(10),
                BackColor = Color.FromArgb(240, 240, 240)
            };
            mainLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 35F));
            mainLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));

            // Top panel with all controls
            FlowLayoutPanel topFlow = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                FlowDirection = FlowDirection.LeftToRight,
                WrapContents = false,
                BackColor = Color.Transparent,
                Height = 35
            };

            Label categoryLabel = new Label
            {
                Text = "What are we crafting?",
                AutoSize = true,
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                Margin = new Padding(0, 8, 10, 0),
                ForeColor = Color.FromArgb(64, 64, 64)
            };

            categorySelector = new ComboBox
            {
                DropDownStyle = ComboBoxStyle.DropDownList,
                Width = 150,
                Height = 25,
                Margin = new Padding(0, 4, 10, 0),
                FlatStyle = FlatStyle.System,
                BackColor = Color.White
            };
            categorySelector.Items.AddRange(new string[] { "Ships", "Craftables" });
            categorySelector.SelectedIndexChanged += CategorySelector_SelectedIndexChanged;

            factionSelector = new ComboBox
            {
                DropDownStyle = ComboBoxStyle.DropDownList,
                Width = 150,
                Height = 25,
                Margin = new Padding(0, 4, 10, 0),
                Visible = false,
                FlatStyle = FlatStyle.System,
                BackColor = Color.White
            };
            factionSelector.Items.AddRange(new string[] { "Human", "Pirate", "Wyrd", "Het", "Precursor" });
            factionSelector.SelectedIndexChanged += FactionSelector_SelectedIndexChanged;

            configButton = new Button
            {
                Text = "⚙️",
                Width = 32,
                Height = 25,
                Margin = new Padding(0, 4, 0, 0),
                FlatStyle = FlatStyle.System,
                Cursor = Cursors.Hand,
                BackColor = Color.White
            };
            configButton.Click += ConfigButton_Click;

            topFlow.Controls.Add(categoryLabel);
            topFlow.Controls.Add(categorySelector);
            topFlow.Controls.Add(factionSelector);
            topFlow.Controls.Add(configButton);

            // Content panel
            contentPanel = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.Transparent
            };

            TableLayoutPanel contentLayout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 2,
                RowCount = 1,
                Margin = new Padding(0, 10, 0, 0),
                BackColor = Color.Transparent
            };
            contentLayout.ColumnStyles.Clear();
            contentLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            contentLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));

            recipeList = new ListBox
            {
                Dock = DockStyle.Fill,
                Font = new Font("Segoe UI", 9F),
                BorderStyle = BorderStyle.FixedSingle,
                BackColor = Color.White,
                SelectionMode = SelectionMode.One,
                IntegralHeight = false,
                HorizontalScrollbar = true
            };
            recipeList.SelectedIndexChanged += RecipeList_SelectedIndexChanged;

            recipeDetailsPanel = new Panel
            {
                Dock = DockStyle.Fill,
                BorderStyle = BorderStyle.FixedSingle,
                BackColor = Color.White,
                Margin = new Padding(5, 0, 0, 0)
            };

            contentLayout.Controls.Add(recipeList, 0, 0);
            contentLayout.Controls.Add(recipeDetailsPanel, 1, 0);
            contentPanel.Controls.Add(contentLayout);

            mainLayout.Controls.Add(topFlow, 0, 0);
            mainLayout.Controls.Add(contentPanel, 0, 1);

            this.Controls.Add(mainLayout);
        }

        private void InitializeRecipes()
        {
            // Existing Mutate Kit recipe
            Recipe mutateKit = new Recipe("Mutate Item Kit", 1);
            mutateKit.AddIngredient("crew", 350);
            mutateKit.AddIngredient("organics", 650);
            mutateKit.AddIngredient("gas", 550);
            mutateKit.AddIngredient("metals", 450);
            mutateKit.AddIngredient("radioactives", 250);
            mutateKit.AddIngredient("darkmatter", 150);
            recipes.Add("Mutate Item Kit", mutateKit);

            // Minor Quality Repair Kit
            Recipe minorKit = new Recipe("Minor Quality Repair Kit", 1);
            minorKit.AddIngredient("crew", 1000);
            minorKit.AddIngredient("organics", 3000);
            minorKit.AddIngredient("gas", 2500);
            minorKit.AddIngredient("metals", 2000);
            minorKit.AddIngredient("radioactives", 1500);
            minorKit.AddIngredient("darkmatter", 1000);
            recipes.Add("Minor Quality Repair Kit", minorKit);

            // Major Quality Repair Kit
            Recipe majorKit = new Recipe("Major Quality Repair Kit", 1);
            majorKit.AddIngredient("crew", 3000);
            majorKit.AddIngredient("organics", 9000);
            majorKit.AddIngredient("gas", 7500);
            majorKit.AddIngredient("metals", 6000);
            majorKit.AddIngredient("radioactives", 4500);
            majorKit.AddIngredient("darkmatter", 3000);
            recipes.Add("Major Quality Repair Kit", majorKit);

            // Ultimate Quality Repair Kit
            Recipe ultimateKit = new Recipe("Ultimate Quality Repair Kit", 1);
            ultimateKit.AddIngredient("crew", 5000);
            ultimateKit.AddIngredient("organics", 15000);
            ultimateKit.AddIngredient("gas", 12500);
            ultimateKit.AddIngredient("metals", 10000);
            ultimateKit.AddIngredient("radioactives", 7500);
            ultimateKit.AddIngredient("darkmatter", 5000);
            recipes.Add("Ultimate Quality Repair Kit", ultimateKit);

            // Nanobot Charge Kit 10
            Recipe nanobot10 = new Recipe("Nanobot Charge Kit 10", 1);
            nanobot10.AddIngredient("crew", 100);
            nanobot10.AddIngredient("organics", 400);
            nanobot10.AddIngredient("gas", 400);
            nanobot10.AddIngredient("metals", 400);
            nanobot10.AddIngredient("radioactives", 100);
            nanobot10.AddIngredient("darkmatter", 100);
            recipes.Add("Nanobot Charge Kit 10", nanobot10);

            // Nanobot Charge Kit 20
            Recipe nanobot20 = new Recipe("Nanobot Charge Kit 20", 1);
            nanobot20.AddIngredient("crew", 175);
            nanobot20.AddIngredient("organics", 800);
            nanobot20.AddIngredient("gas", 800);
            nanobot20.AddIngredient("metals", 800);
            nanobot20.AddIngredient("radioactives", 175);
            nanobot20.AddIngredient("darkmatter", 175);
            recipes.Add("Nanobot Charge Kit 20", nanobot20);

            // Nanobot Charge Kit 30
            Recipe nanobot30 = new Recipe("Nanobot Charge Kit 30", 1);
            nanobot30.AddIngredient("crew", 250);
            nanobot30.AddIngredient("organics", 1200);
            nanobot30.AddIngredient("gas", 1200);
            nanobot30.AddIngredient("metals", 1200);
            nanobot30.AddIngredient("radioactives", 250);
            nanobot30.AddIngredient("darkmatter", 250);
            recipes.Add("Nanobot Charge Kit 30", nanobot30);

            // Ultimate Ship Mod
            Recipe ultimateShipMod = new Recipe("Ultimate Ship Mod", 1);
            ultimateShipMod.RequiresFactionSelection = true;
            ultimateShipMod.AddFactionRequirement("human oblivion fragment", 10);
            ultimateShipMod.AddFactionRequirement("het oblivion fragment", 10);
            ultimateShipMod.AddFactionRequirement("wyrd oblivion fragment", 10);
            ultimateShipMod.AddFactionRequirement("precursor oblivion fragment", 10);
            recipes.Add("Ultimate Ship Mod", ultimateShipMod);

            // Interdictor Carrier Ship Mod
            Recipe interdictorMod = new Recipe("Interdictor Carrier Ship Mod", 1);
            interdictorMod.RequiresFactionSelection = true;
            interdictorMod.AddIngredient("crew", 2000);
            interdictorMod.AddIngredient("organics", 2000);
            interdictorMod.AddIngredient("gas", 2000);
            interdictorMod.AddIngredient("metals", 2000);
            interdictorMod.AddIngredient("radioactives", 2000);
            interdictorMod.AddIngredient("darkmatter", 2000);
            interdictorMod.AddFactionRequirement("human relic", 150);
            interdictorMod.AddFactionRequirement("het relic", 150);
            interdictorMod.AddFactionRequirement("wyrd relic", 150);
            interdictorMod.AddFactionRequirement("precursor relic", 150);
            interdictorMod.AddFactionRequirement("human oblivion fragment", 40);
            interdictorMod.AddFactionRequirement("het oblivion fragment", 40);
            interdictorMod.AddFactionRequirement("wyrd oblivion fragment", 40);
            interdictorMod.AddFactionRequirement("precursor oblivion fragment", 40);
            recipes.Add("Interdictor Carrier Ship Mod", interdictorMod);

            // Human Ships with resources
            Recipe shuttle = new Recipe("Shuttle (Human)", 1);
            shuttle.AddIngredient("crew", 205);
            shuttle.AddIngredient("organics", 170);
            shuttle.AddIngredient("gas", 135);
            shuttle.AddIngredient("metals", 205);
            shuttle.AddIngredient("radioactives", 175);
            shuttle.AddIngredient("darkmatter", 120);
            recipes.Add("Shuttle (Human)", shuttle);

            Recipe frigate = new Recipe("Frigate (Human)", 1);
            frigate.AddIngredient("crew", 260);
            frigate.AddIngredient("organics", 215);
            frigate.AddIngredient("gas", 170);
            frigate.AddIngredient("metals", 260);
            frigate.AddIngredient("radioactives", 225);
            frigate.AddIngredient("darkmatter", 155);
            recipes.Add("Frigate (Human)", frigate);

            Recipe assassin = new Recipe("Assassin (Human)", 1);
            assassin.AddIngredient("crew", 1970);
            assassin.AddIngredient("organics", 1630);
            assassin.AddIngredient("gas", 1290);
            assassin.AddIngredient("metals", 1970);
            assassin.AddIngredient("radioactives", 1700);
            assassin.AddIngredient("darkmatter", 1155);
            assassin.AddIngredient("human relic", 1);
            assassin.AddIngredient("skull", 75);
            recipes.Add("Assassin (Human)", assassin);

            Recipe cruiser = new Recipe("Cruiser (Human)", 1);
            cruiser.AddIngredient("crew", 405);
            cruiser.AddIngredient("organics", 335);
            cruiser.AddIngredient("gas", 265);
            cruiser.AddIngredient("metals", 405);
            cruiser.AddIngredient("radioactives", 350);
            cruiser.AddIngredient("darkmatter", 240);
            recipes.Add("Cruiser (Human)", cruiser);

            Recipe destroyer = new Recipe("Destroyer (Human)", 1);
            destroyer.AddIngredient("crew", 435);
            destroyer.AddIngredient("organics", 360);
            destroyer.AddIngredient("gas", 285);
            destroyer.AddIngredient("metals", 435);
            destroyer.AddIngredient("radioactives", 375);
            destroyer.AddIngredient("darkmatter", 255);
            destroyer.AddIngredient("skull", 10);
            recipes.Add("Destroyer (Human)", destroyer);

            Recipe battleship = new Recipe("Battleship (Human)", 1);
            battleship.AddIngredient("crew", 520);
            battleship.AddIngredient("organics", 430);
            battleship.AddIngredient("gas", 340);
            battleship.AddIngredient("metals", 520);
            battleship.AddIngredient("radioactives", 450);
            battleship.AddIngredient("darkmatter", 305);
            battleship.AddIngredient("skull", 25);
            recipes.Add("Battleship (Human)", battleship);

            Recipe flagship = new Recipe("Flagship (Human)", 1);
            flagship.AddIngredient("crew", 2610);
            flagship.AddIngredient("organics", 2160);
            flagship.AddIngredient("gas", 1710);
            flagship.AddIngredient("metals", 2610);
            flagship.AddIngredient("radioactives", 2250);
            flagship.AddIngredient("darkmatter", 1530);
            flagship.AddIngredient("skull", 50);
            recipes.Add("Flagship (Human)", flagship);

            Recipe carrier = new Recipe("Carrier (Human)", 1);
            carrier.AddIngredient("crew", 3450);
            carrier.AddIngredient("organics", 2855);
            carrier.AddIngredient("gas", 2260);
            carrier.AddIngredient("metals", 3450);
            carrier.AddIngredient("radioactives", 2975);
            carrier.AddIngredient("darkmatter", 2025);
            carrier.AddIngredient("human relic", 8);
            carrier.AddIngredient("skull", 250);
            recipes.Add("Carrier (Human)", carrier);

            Recipe hades = new Recipe("Hades (Human)", 1);
            hades.AddIngredient("crew", 3450);
            hades.AddIngredient("organics", 2865);
            hades.AddIngredient("gas", 2260);
            hades.AddIngredient("metals", 3450);
            hades.AddIngredient("radioactives", 2975);
            hades.AddIngredient("darkmatter", 2025);
            hades.AddIngredient("human relic", 2);
            hades.AddIngredient("skull", 125);
            recipes.Add("Hades (Human)", hades);

            Recipe emperor = new Recipe("Emperor (Human)", 1);
            emperor.AddIngredient("crew", 5945);
            emperor.AddIngredient("organics", 4920);
            emperor.AddIngredient("gas", 3895);
            emperor.AddIngredient("metals", 5945);
            emperor.AddIngredient("radioactives", 5125);
            emperor.AddIngredient("darkmatter", 3485);
            emperor.AddIngredient("human relic", 25);
            emperor.AddIngredient("skull", 500);
            recipes.Add("Emperor (Human)", emperor);

            // Pirate Ships with complete resources
            Recipe pirateShuttle = new Recipe("Shuttle (Pirate)", 1);
            pirateShuttle.AddIngredient("crew", 350);
            pirateShuttle.AddIngredient("organics", 265);
            pirateShuttle.AddIngredient("gas", 210);
            pirateShuttle.AddIngredient("metals", 350);
            pirateShuttle.AddIngredient("radioactives", 275);
            pirateShuttle.AddIngredient("darkmatter", 185);
            recipes.Add("Shuttle (Pirate)", pirateShuttle);

            Recipe pirateFrigate = new Recipe("Frigate (Pirate)", 1);
            pirateFrigate.AddIngredient("crew", 415);
            pirateFrigate.AddIngredient("organics", 310);
            pirateFrigate.AddIngredient("gas", 245);
            pirateFrigate.AddIngredient("metals", 415);
            pirateFrigate.AddIngredient("radioactives", 325);
            pirateFrigate.AddIngredient("darkmatter", 220);
            recipes.Add("Frigate (Pirate)", pirateFrigate);

            Recipe pirateAssassin = new Recipe("Assassin (Pirate)", 1);
            pirateAssassin.AddIngredient("crew", 2305);
            pirateAssassin.AddIngredient("organics", 1730);
            pirateAssassin.AddIngredient("gas", 1370);
            pirateAssassin.AddIngredient("metals", 2305);
            pirateAssassin.AddIngredient("radioactives", 1800);
            pirateAssassin.AddIngredient("darkmatter", 1225);
            pirateAssassin.AddIngredient("human relic", 1);
            pirateAssassin.AddIngredient("skull", 150);
            recipes.Add("Assassin (Pirate)", pirateAssassin);

            Recipe pirateCruiser = new Recipe("Cruiser (Pirate)", 1);
            pirateCruiser.AddIngredient("crew", 575);
            pirateCruiser.AddIngredient("organics", 430);
            pirateCruiser.AddIngredient("gas", 340);
            pirateCruiser.AddIngredient("metals", 575);
            pirateCruiser.AddIngredient("radioactives", 450);
            pirateCruiser.AddIngredient("darkmatter", 305);
            recipes.Add("Cruiser (Pirate)", pirateCruiser);

            Recipe pirateDestroyer = new Recipe("Destroyer (Pirate)", 1);
            pirateDestroyer.AddIngredient("crew", 640);
            pirateDestroyer.AddIngredient("organics", 480);
            pirateDestroyer.AddIngredient("gas", 380);
            pirateDestroyer.AddIngredient("metals", 640);
            pirateDestroyer.AddIngredient("radioactives", 500);
            pirateDestroyer.AddIngredient("darkmatter", 340);
            pirateDestroyer.AddIngredient("skull", 20);
            recipes.Add("Destroyer (Pirate)", pirateDestroyer);

            Recipe pirateBattleship = new Recipe("Battleship (Pirate)", 1);
            pirateBattleship.AddIngredient("crew", 735);
            pirateBattleship.AddIngredient("organics", 550);
            pirateBattleship.AddIngredient("gas", 435);
            pirateBattleship.AddIngredient("metals", 735);
            pirateBattleship.AddIngredient("radioactives", 575);
            pirateBattleship.AddIngredient("darkmatter", 390);
            pirateBattleship.AddIngredient("skull", 50);
            recipes.Add("Battleship (Pirate)", pirateBattleship);

            Recipe pirateFlagship = new Recipe("Flagship (Pirate)", 1);
            pirateFlagship.AddIngredient("crew", 3170);
            pirateFlagship.AddIngredient("organics", 2375);
            pirateFlagship.AddIngredient("gas", 1880);
            pirateFlagship.AddIngredient("metals", 3170);
            pirateFlagship.AddIngredient("radioactives", 2475);
            pirateFlagship.AddIngredient("darkmatter", 1685);
            pirateFlagship.AddIngredient("skull", 100);
            recipes.Add("Flagship (Pirate)", pirateFlagship);

            Recipe pirateCarrier = new Recipe("Carrier (Pirate)", 1);
            pirateCarrier.AddIngredient("crew", 4160);
            pirateCarrier.AddIngredient("organics", 3120);
            pirateCarrier.AddIngredient("gas", 2470);
            pirateCarrier.AddIngredient("metals", 4160);
            pirateCarrier.AddIngredient("radioactives", 3250);
            pirateCarrier.AddIngredient("darkmatter", 2210);
            pirateCarrier.AddIngredient("human relic", 15);
            pirateCarrier.AddIngredient("skull", 500);
            recipes.Add("Carrier (Pirate)", pirateCarrier);

            Recipe pirateHades = new Recipe("Hades (Pirate)", 1);
            pirateHades.AddIngredient("crew", 4160);
            pirateHades.AddIngredient("organics", 3120);
            pirateHades.AddIngredient("gas", 2470);
            pirateHades.AddIngredient("metals", 4160);
            pirateHades.AddIngredient("radioactives", 3250);
            pirateHades.AddIngredient("darkmatter", 2210);
            pirateHades.AddIngredient("human relic", 3);
            pirateHades.AddIngredient("skull", 250);
            recipes.Add("Hades (Pirate)", pirateHades);

            Recipe pirateEmperor = new Recipe("Emperor (Pirate)", 1);
            pirateEmperor.AddIngredient("crew", 7040);
            pirateEmperor.AddIngredient("organics", 5280);
            pirateEmperor.AddIngredient("gas", 4180);
            pirateEmperor.AddIngredient("metals", 7040);
            pirateEmperor.AddIngredient("radioactives", 5500);
            pirateEmperor.AddIngredient("darkmatter", 3740);
            pirateEmperor.AddIngredient("human relic", 50);
            pirateEmperor.AddIngredient("skull", 1000);
            recipes.Add("Emperor (Pirate)", pirateEmperor);

            // Wyrd Ships
            Recipe wyrdSpitfire = new Recipe("Spitfire (Wyrd)", 1);
            wyrdSpitfire.AddIngredient("crew", 450);
            wyrdSpitfire.AddIngredient("organics", 335);
            wyrdSpitfire.AddIngredient("gas", 310);
            wyrdSpitfire.AddIngredient("metals", 405);
            wyrdSpitfire.AddIngredient("radioactives", 390);
            wyrdSpitfire.AddIngredient("darkmatter", 240);
            recipes.Add("Spitfire (Wyrd)", wyrdSpitfire);

            Recipe wyrdScout = new Recipe("Scout (Wyrd)", 1);
            wyrdScout.AddIngredient("crew", 510);
            wyrdScout.AddIngredient("organics", 385);
            wyrdScout.AddIngredient("gas", 350);
            wyrdScout.AddIngredient("metals", 465);
            wyrdScout.AddIngredient("radioactives", 450);
            wyrdScout.AddIngredient("darkmatter", 270);
            recipes.Add("Scout (Wyrd)", wyrdScout);

            Recipe wyrdAssassin = new Recipe("Assassin (Wyrd)", 1);
            wyrdAssassin.AddIngredient("crew", 2815);
            wyrdAssassin.AddIngredient("organics", 2110);
            wyrdAssassin.AddIngredient("gas", 1935);
            wyrdAssassin.AddIngredient("metals", 2550);
            wyrdAssassin.AddIngredient("radioactives", 2465);
            wyrdAssassin.AddIngredient("darkmatter", 1495);
            wyrdAssassin.AddIngredient("wyrd relic", 1);
            wyrdAssassin.AddIngredient("skull", 150);
            recipes.Add("Assassin (Wyrd)", wyrdAssassin);

            Recipe wyrdFlayer = new Recipe("Flayer (Wyrd)", 1);
            wyrdFlayer.AddIngredient("crew", 705);
            wyrdFlayer.AddIngredient("organics", 530);
            wyrdFlayer.AddIngredient("gas", 485);
            wyrdFlayer.AddIngredient("metals", 640);
            wyrdFlayer.AddIngredient("radioactives", 615);
            wyrdFlayer.AddIngredient("darkmatter", 375);
            recipes.Add("Flayer (Wyrd)", wyrdFlayer);

            Recipe wyrdDevastator = new Recipe("Devastator (Wyrd)", 1);
            wyrdDevastator.AddIngredient("crew", 770);
            wyrdDevastator.AddIngredient("organics", 575);
            wyrdDevastator.AddIngredient("gas", 530);
            wyrdDevastator.AddIngredient("metals", 695);
            wyrdDevastator.AddIngredient("radioactives", 670);
            wyrdDevastator.AddIngredient("darkmatter", 410);
            wyrdDevastator.AddIngredient("skull", 20);
            recipes.Add("Devastator (Wyrd)", wyrdDevastator);

            Recipe wyrdInvader = new Recipe("Invader (Wyrd)", 1);
            wyrdInvader.AddIngredient("crew", 895);
            wyrdInvader.AddIngredient("organics", 670);
            wyrdInvader.AddIngredient("gas", 615);
            wyrdInvader.AddIngredient("metals", 810);
            wyrdInvader.AddIngredient("radioactives", 785);
            wyrdInvader.AddIngredient("darkmatter", 475);
            wyrdInvader.AddIngredient("skull", 50);
            recipes.Add("Invader (Wyrd)", wyrdInvader);

            Recipe wyrdExecutioner = new Recipe("Executioner (Wyrd)", 1);
            wyrdExecutioner.AddIngredient("crew", 3710);
            wyrdExecutioner.AddIngredient("organics", 2785);
            wyrdExecutioner.AddIngredient("gas", 2550);
            wyrdExecutioner.AddIngredient("metals", 3365);
            wyrdExecutioner.AddIngredient("radioactives", 3250);
            wyrdExecutioner.AddIngredient("darkmatter", 1970);
            wyrdExecutioner.AddIngredient("skull", 100);
            recipes.Add("Executioner (Wyrd)", wyrdExecutioner);

            Recipe wyrdCarrier = new Recipe("Carrier (Wyrd)", 1);
            wyrdCarrier.AddIngredient("crew", 4800);
            wyrdCarrier.AddIngredient("organics", 3600);
            wyrdCarrier.AddIngredient("gas", 3300);
            wyrdCarrier.AddIngredient("metals", 4350);
            wyrdCarrier.AddIngredient("radioactives", 4200);
            wyrdCarrier.AddIngredient("darkmatter", 2550);
            wyrdCarrier.AddIngredient("wyrd relic", 15);
            wyrdCarrier.AddIngredient("skull", 500);
            recipes.Add("Carrier (Wyrd)", wyrdCarrier);

            Recipe wyrdReaper = new Recipe("Reaper (Wyrd)", 1);
            wyrdReaper.AddIngredient("crew", 4800);
            wyrdReaper.AddIngredient("organics", 3600);
            wyrdReaper.AddIngredient("gas", 3300);
            wyrdReaper.AddIngredient("metals", 4350);
            wyrdReaper.AddIngredient("radioactives", 4200);
            wyrdReaper.AddIngredient("darkmatter", 2550);
            wyrdReaper.AddIngredient("wyrd relic", 3);
            wyrdReaper.AddIngredient("skull", 250);
            recipes.Add("Reaper (Wyrd)", wyrdReaper);

            Recipe wyrdEmperor = new Recipe("Emperor (Wyrd)", 1);
            wyrdEmperor.AddIngredient("crew", 8095);
            wyrdEmperor.AddIngredient("organics", 6070);
            wyrdEmperor.AddIngredient("gas", 5565);
            wyrdEmperor.AddIngredient("metals", 7335);
            wyrdEmperor.AddIngredient("radioactives", 7085);
            wyrdEmperor.AddIngredient("darkmatter", 4300);
            wyrdEmperor.AddIngredient("wyrd relic", 50);
            wyrdEmperor.AddIngredient("skull", 1000);
            recipes.Add("Emperor (Wyrd)", wyrdEmperor);

            // Het Ships
            Recipe hetLarva = new Recipe("Larva (Het)", 1);
            hetLarva.AddIngredient("crew", 375);
            hetLarva.AddIngredient("organics", 350);
            hetLarva.AddIngredient("gas", 245);
            hetLarva.AddIngredient("metals", 375);
            hetLarva.AddIngredient("radioactives", 325);
            hetLarva.AddIngredient("darkmatter", 220);
            recipes.Add("Larva (Het)", hetLarva);

            Recipe hetStinger = new Recipe("Stinger (Het)", 1);
            hetStinger.AddIngredient("crew", 435);
            hetStinger.AddIngredient("organics", 405);
            hetStinger.AddIngredient("gas", 285);
            hetStinger.AddIngredient("metals", 435);
            hetStinger.AddIngredient("radioactives", 375);
            hetStinger.AddIngredient("darkmatter", 255);
            recipes.Add("Stinger (Het)", hetStinger);

            Recipe hetHornet = new Recipe("Hornet (Het)", 1);
            hetHornet.AddIngredient("crew", 2320);
            hetHornet.AddIngredient("organics", 2160);
            hetHornet.AddIngredient("gas", 1520);
            hetHornet.AddIngredient("metals", 2320);
            hetHornet.AddIngredient("radioactives", 2000);
            hetHornet.AddIngredient("darkmatter", 1360);
            hetHornet.AddIngredient("het relic", 1);
            hetHornet.AddIngredient("skull", 150);
            recipes.Add("Hornet (Het)", hetHornet);

            Recipe hetDefender = new Recipe("Defender (Het)", 1);
            hetDefender.AddIngredient("crew", 580);
            hetDefender.AddIngredient("organics", 540);
            hetDefender.AddIngredient("gas", 380);
            hetDefender.AddIngredient("metals", 580);
            hetDefender.AddIngredient("radioactives", 500);
            hetDefender.AddIngredient("darkmatter", 340);
            recipes.Add("Defender (Het)", hetDefender);

            Recipe hetWarrior = new Recipe("Warrior (Het)", 1);
            hetWarrior.AddIngredient("crew", 640);
            hetWarrior.AddIngredient("organics", 595);
            hetWarrior.AddIngredient("gas", 420);
            hetWarrior.AddIngredient("metals", 640);
            hetWarrior.AddIngredient("radioactives", 550);
            hetWarrior.AddIngredient("darkmatter", 375);
            hetWarrior.AddIngredient("skull", 20);
            recipes.Add("Warrior (Het)", hetWarrior);

            Recipe hetPredator = new Recipe("Predator (Het)", 1);
            hetPredator.AddIngredient("crew", 725);
            hetPredator.AddIngredient("organics", 675);
            hetPredator.AddIngredient("gas", 475);
            hetPredator.AddIngredient("metals", 725);
            hetPredator.AddIngredient("radioactives", 625);
            hetPredator.AddIngredient("darkmatter", 425);
            hetPredator.AddIngredient("skull", 50);
            recipes.Add("Predator (Het)", hetPredator);

            Recipe hetHunter = new Recipe("Hunter (Het)", 1);
            hetHunter.AddIngredient("crew", 3105);
            hetHunter.AddIngredient("organics", 2890);
            hetHunter.AddIngredient("gas", 2035);
            hetHunter.AddIngredient("metals", 3105);
            hetHunter.AddIngredient("radioactives", 2675);
            hetHunter.AddIngredient("darkmatter", 1820);
            hetHunter.AddIngredient("skull", 100);
            recipes.Add("Hunter (Het)", hetHunter);

            Recipe hetQueen = new Recipe("Queen (Het)", 1);
            hetQueen.AddIngredient("crew", 4060);
            hetQueen.AddIngredient("organics", 3780);
            hetQueen.AddIngredient("gas", 2660);
            hetQueen.AddIngredient("metals", 4060);
            hetQueen.AddIngredient("radioactives", 3500);
            hetQueen.AddIngredient("darkmatter", 2380);
            hetQueen.AddIngredient("het relic", 15);
            hetQueen.AddIngredient("skull", 500);
            recipes.Add("Queen (Het)", hetQueen);

            Recipe hetKing = new Recipe("King (Het)", 1);
            hetKing.AddIngredient("crew", 4060);
            hetKing.AddIngredient("organics", 3780);
            hetKing.AddIngredient("gas", 2660);
            hetKing.AddIngredient("metals", 4060);
            hetKing.AddIngredient("radioactives", 3500);
            hetKing.AddIngredient("darkmatter", 2380);
            hetKing.AddIngredient("het relic", 3);
            hetKing.AddIngredient("skull", 250);
            recipes.Add("King (Het)", hetKing);

            Recipe hetGodhead = new Recipe("Godhead (Het)", 1);
            hetGodhead.AddIngredient("crew", 6845);
            hetGodhead.AddIngredient("organics", 6370);
            hetGodhead.AddIngredient("gas", 4485);
            hetGodhead.AddIngredient("metals", 6845);
            hetGodhead.AddIngredient("radioactives", 5900);
            hetGodhead.AddIngredient("darkmatter", 4010);
            hetGodhead.AddIngredient("het relic", 50);
            hetGodhead.AddIngredient("skull", 1000);
            recipes.Add("Godhead (Het)", hetGodhead);

            // Precursor Ships
            Recipe precursorShuttle = new Recipe("Shuttle (Precursor)", 1);
            precursorShuttle.AddIngredient("crew", 545);
            precursorShuttle.AddIngredient("organics", 410);
            precursorShuttle.AddIngredient("gas", 325);
            precursorShuttle.AddIngredient("metals", 495);
            precursorShuttle.AddIngredient("radioactives", 475);
            precursorShuttle.AddIngredient("darkmatter", 340);
            recipes.Add("Shuttle (Precursor)", precursorShuttle);

            Recipe precursorFrigate = new Recipe("Frigate (Precursor)", 1);
            precursorFrigate.AddIngredient("crew", 610);
            precursorFrigate.AddIngredient("organics", 455);
            precursorFrigate.AddIngredient("gas", 360);
            precursorFrigate.AddIngredient("metals", 550);
            precursorFrigate.AddIngredient("radioactives", 530);
            precursorFrigate.AddIngredient("darkmatter", 380);
            recipes.Add("Frigate (Precursor)", precursorFrigate);

            Recipe precursorAssassin = new Recipe("Assassin (Precursor)", 1);
            precursorAssassin.AddIngredient("crew", 3230);
            precursorAssassin.AddIngredient("organics", 2425);
            precursorAssassin.AddIngredient("gas", 1920);
            precursorAssassin.AddIngredient("metals", 2930);
            precursorAssassin.AddIngredient("radioactives", 2630);
            precursorAssassin.AddIngredient("darkmatter", 2020);
            precursorAssassin.AddIngredient("precursor relic", 1);
            precursorAssassin.AddIngredient("skull", 150);
            recipes.Add("Assassin (Precursor)", precursorAssassin);

            Recipe precursorCruiser = new Recipe("Cruiser (Precursor)", 1);
            precursorCruiser.AddIngredient("crew", 800);
            precursorCruiser.AddIngredient("organics", 600);
            precursorCruiser.AddIngredient("gas", 475);
            precursorCruiser.AddIngredient("metals", 725);
            precursorCruiser.AddIngredient("radioactives", 700);
            precursorCruiser.AddIngredient("darkmatter", 500);
            recipes.Add("Cruiser (Precursor)", precursorCruiser);

            Recipe precursorDespoiler = new Recipe("Despoiler (Precursor)", 1);
            precursorDespoiler.AddIngredient("crew", 895);
            precursorDespoiler.AddIngredient("organics", 670);
            precursorDespoiler.AddIngredient("gas", 530);
            precursorDespoiler.AddIngredient("metals", 810);
            precursorDespoiler.AddIngredient("radioactives", 785);
            precursorDespoiler.AddIngredient("darkmatter", 560);
            precursorDespoiler.AddIngredient("skull", 20);
            recipes.Add("Despoiler (Precursor)", precursorDespoiler);

            Recipe precursorTitan = new Recipe("Titan (Precursor)", 1);
            precursorTitan.AddIngredient("crew", 990);
            precursorTitan.AddIngredient("organics", 745);
            precursorTitan.AddIngredient("gas", 590);
            precursorTitan.AddIngredient("metals", 900);
            precursorTitan.AddIngredient("radioactives", 870);
            precursorTitan.AddIngredient("darkmatter", 620);
            precursorTitan.AddIngredient("skull", 50);
            recipes.Add("Titan (Precursor)", precursorTitan);

            Recipe precursorDreadnought = new Recipe("Dreadnought (Precursor)", 1);
            precursorDreadnought.AddIngredient("crew", 4160);
            precursorDreadnought.AddIngredient("organics", 3120);
            precursorDreadnought.AddIngredient("gas", 2470);
            precursorDreadnought.AddIngredient("metals", 3770);
            precursorDreadnought.AddIngredient("radioactives", 3640);
            precursorDreadnought.AddIngredient("darkmatter", 2600);
            precursorDreadnought.AddIngredient("skull", 100);
            recipes.Add("Dreadnought (Precursor)", precursorDreadnought);

            Recipe precursorCarrier = new Recipe("Carrier (Precursor)", 1);
            precursorCarrier.AddIngredient("crew", 5375);
            precursorCarrier.AddIngredient("organics", 4030);
            precursorCarrier.AddIngredient("gas", 3190);
            precursorCarrier.AddIngredient("metals", 4870);
            precursorCarrier.AddIngredient("radioactives", 4705);
            precursorCarrier.AddIngredient("darkmatter", 3360);
            precursorCarrier.AddIngredient("precursor relic", 15);
            precursorCarrier.AddIngredient("skull", 500);
            recipes.Add("Carrier (Precursor)", precursorCarrier);

            Recipe precursorTerminus = new Recipe("Terminus (Precursor)", 1);
            precursorTerminus.AddIngredient("crew", 5375);
            precursorTerminus.AddIngredient("organics", 4030);
            precursorTerminus.AddIngredient("gas", 3190);
            precursorTerminus.AddIngredient("metals", 4870);
            precursorTerminus.AddIngredient("radioactives", 4705);
            precursorTerminus.AddIngredient("darkmatter", 3360);
            precursorTerminus.AddIngredient("precursor relic", 3);
            precursorTerminus.AddIngredient("skull", 250);
            recipes.Add("Terminus (Precursor)", precursorTerminus);

            Recipe precursorEmperor = new Recipe("Emperor (Precursor)", 1);
            precursorEmperor.AddIngredient("crew", 9025);
            precursorEmperor.AddIngredient("organics", 6770);
            precursorEmperor.AddIngredient("gas", 5360);
            precursorEmperor.AddIngredient("metals", 8190);
            precursorEmperor.AddIngredient("radioactives", 7895);
            precursorEmperor.AddIngredient("darkmatter", 5640);
            precursorEmperor.AddIngredient("precursor relic", 50);
            precursorEmperor.AddIngredient("skull", 1000);
            recipes.Add("Emperor (Precursor)", precursorEmperor);
        }

        private void ConfigButton_Click(object sender, EventArgs e)
        {
            using (var configForm = new ResourceConfigForm(resourceManager))
            {
                configForm.ShowDialog();
            }
        }

        private void CategorySelector_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedCategory = categorySelector.SelectedItem?.ToString();
            factionSelector.Visible = selectedCategory == "Ships";
            if (factionSelector.Visible)
            {
                factionSelector.SelectedIndex = -1;
            }
            UpdateRecipeList(selectedCategory);
        }

        private void UpdateRecipeList(string category)
        {
            recipeList.Items.Clear();
            recipeDetailsPanel.Controls.Clear();

            if (category == "Craftables")
            {
                recipeList.Items.Add("Mutate Item Kit");
                recipeList.Items.Add("Minor Quality Repair Kit");
                recipeList.Items.Add("Major Quality Repair Kit");
                recipeList.Items.Add("Ultimate Quality Repair Kit");
                recipeList.Items.Add("Nanobot Charge Kit 10");
                recipeList.Items.Add("Nanobot Charge Kit 20");
                recipeList.Items.Add("Nanobot Charge Kit 30");
                recipeList.Items.Add("Ultimate Ship Mod");
                recipeList.Items.Add("Interdictor Carrier Ship Mod");
            }
            else if (category == "Ships")
            {
                string selectedFaction = factionSelector.SelectedItem?.ToString();
                if (selectedFaction == "Human")
                {
                    recipeList.Items.Add("Shuttle (Human)");
                    recipeList.Items.Add("Frigate (Human)");
                    recipeList.Items.Add("Assassin (Human)");
                    recipeList.Items.Add("Cruiser (Human)");
                    recipeList.Items.Add("Destroyer (Human)");
                    recipeList.Items.Add("Battleship (Human)");
                    recipeList.Items.Add("Flagship (Human)");
                    recipeList.Items.Add("Carrier (Human)");
                    recipeList.Items.Add("Hades (Human)");
                    recipeList.Items.Add("Emperor (Human)");
                }
                else if (selectedFaction == "Pirate")
                {
                    recipeList.Items.Add("Shuttle (Pirate)");
                    recipeList.Items.Add("Frigate (Pirate)");
                    recipeList.Items.Add("Assassin (Pirate)");
                    recipeList.Items.Add("Cruiser (Pirate)");
                    recipeList.Items.Add("Destroyer (Pirate)");
                    recipeList.Items.Add("Battleship (Pirate)");
                    recipeList.Items.Add("Flagship (Pirate)");
                    recipeList.Items.Add("Carrier (Pirate)");
                    recipeList.Items.Add("Hades (Pirate)");
                    recipeList.Items.Add("Emperor (Pirate)");
                }

                else if (selectedFaction == "Wyrd")
                {
                    recipeList.Items.Add("Spitfire (Wyrd)");
                    recipeList.Items.Add("Scout (Wyrd)");
                    recipeList.Items.Add("Assassin (Wyrd)");
                    recipeList.Items.Add("Flayer (Wyrd)");
                    recipeList.Items.Add("Devastator (Wyrd)");
                    recipeList.Items.Add("Invader (Wyrd)");
                    recipeList.Items.Add("Executioner (Wyrd)");
                    recipeList.Items.Add("Carrier (Wyrd)");
                    recipeList.Items.Add("Reaper (Wyrd)");
                    recipeList.Items.Add("Emperor (Wyrd)");
                }

                else if (selectedFaction == "Het")
                {
                    recipeList.Items.Add("Larva (Het)");
                    recipeList.Items.Add("Stinger (Het)");
                    recipeList.Items.Add("Hornet (Het)");
                    recipeList.Items.Add("Defender (Het)");
                    recipeList.Items.Add("Warrior (Het)");
                    recipeList.Items.Add("Predator (Het)");
                    recipeList.Items.Add("Hunter (Het)");
                    recipeList.Items.Add("Queen (Het)");
                    recipeList.Items.Add("King (Het)");
                    recipeList.Items.Add("Godhead (Het)");
                }

                else if (selectedFaction == "Precursor")
                {
                    recipeList.Items.Add("Shuttle (Precursor)");
                    recipeList.Items.Add("Frigate (Precursor)");
                    recipeList.Items.Add("Assassin (Precursor)");
                    recipeList.Items.Add("Cruiser (Precursor)");
                    recipeList.Items.Add("Despoiler (Precursor)");
                    recipeList.Items.Add("Titan (Precursor)");
                    recipeList.Items.Add("Dreadnought (Precursor)");
                    recipeList.Items.Add("Carrier (Precursor)");
                    recipeList.Items.Add("Terminus (Precursor)");
                    recipeList.Items.Add("Emperor (Precursor)");
                }
            }
        }

        private void FactionSelector_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedFaction = factionSelector.SelectedItem?.ToString();
            recipeList.Items.Clear();
            recipeDetailsPanel.Controls.Clear();
            
            if (selectedFaction == "Human")
            {
                recipeList.Items.Add("Shuttle (Human)");
                recipeList.Items.Add("Frigate (Human)");
                recipeList.Items.Add("Assassin (Human)");
                recipeList.Items.Add("Cruiser (Human)");
                recipeList.Items.Add("Destroyer (Human)");
                recipeList.Items.Add("Battleship (Human)");
                recipeList.Items.Add("Flagship (Human)");
                recipeList.Items.Add("Carrier (Human)");
                recipeList.Items.Add("Hades (Human)");
                recipeList.Items.Add("Emperor (Human)");
            }

            else if (selectedFaction == "Pirate")
            {
                recipeList.Items.Add("Shuttle (Pirate)");
                recipeList.Items.Add("Frigate (Pirate)");
                recipeList.Items.Add("Assassin (Pirate)");
                recipeList.Items.Add("Cruiser (Pirate)");
                recipeList.Items.Add("Destroyer (Pirate)");
                recipeList.Items.Add("Battleship (Pirate)");
                recipeList.Items.Add("Flagship (Pirate)");
                recipeList.Items.Add("Carrier (Pirate)");
                recipeList.Items.Add("Hades (Pirate)");
                recipeList.Items.Add("Emperor (Pirate)");
            }

            else if (selectedFaction == "Wyrd")
            {
                recipeList.Items.Add("Spitfire (Wyrd)");
                recipeList.Items.Add("Scout (Wyrd)");
                recipeList.Items.Add("Assassin (Wyrd)");
                recipeList.Items.Add("Flayer (Wyrd)");
                recipeList.Items.Add("Devastator (Wyrd)");
                recipeList.Items.Add("Invader (Wyrd)");
                recipeList.Items.Add("Executioner (Wyrd)");
                recipeList.Items.Add("Carrier (Wyrd)");
                recipeList.Items.Add("Reaper (Wyrd)");
                recipeList.Items.Add("Emperor (Wyrd)");
            }

            else if (selectedFaction == "Het")
            {
                recipeList.Items.Add("Larva (Het)");
                recipeList.Items.Add("Stinger (Het)");
                recipeList.Items.Add("Hornet (Het)");
                recipeList.Items.Add("Defender (Het)");
                recipeList.Items.Add("Warrior (Het)");
                recipeList.Items.Add("Predator (Het)");
                recipeList.Items.Add("Hunter (Het)");
                recipeList.Items.Add("Queen (Het)");
                recipeList.Items.Add("King (Het)");
                recipeList.Items.Add("Godhead (Het)");
            }

            else if (selectedFaction == "Precursor")
            {
                recipeList.Items.Add("Shuttle (Precursor)");
                recipeList.Items.Add("Frigate (Precursor)");
                recipeList.Items.Add("Assassin (Precursor)");
                recipeList.Items.Add("Cruiser (Precursor)");
                recipeList.Items.Add("Despoiler (Precursor)");
                recipeList.Items.Add("Titan (Precursor)");
                recipeList.Items.Add("Dreadnought (Precursor)");
                recipeList.Items.Add("Carrier (Precursor)");
                recipeList.Items.Add("Terminus (Precursor)");
                recipeList.Items.Add("Emperor (Precursor)");
            }
        }

        private void RecipeList_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedRecipe = recipeList.SelectedItem?.ToString();
            if (selectedRecipe != null && recipes.ContainsKey(selectedRecipe))
            {
                DisplayRecipeDetails(recipes[selectedRecipe]);
            }
        }

        private void DisplayRecipeDetails(Recipe recipe)
        {
            recipeDetailsPanel.Controls.Clear();

            FlowLayoutPanel detailsLayout = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                FlowDirection = FlowDirection.TopDown,
                AutoScroll = true,
                Padding = new Padding(10)
            };

            Label titleLabel = new Label
            {
                Text = recipe.Name,
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                AutoSize = true,
                Margin = new Padding(0, 0, 0, 10),
                ForeColor = Color.FromArgb(64, 64, 64)
            };
            detailsLayout.Controls.Add(titleLabel);

            // Declare modChoice here
            ComboBox modChoice = new ComboBox
            {
                DropDownStyle = ComboBoxStyle.DropDownList,
                Width = 150,
                Margin = new Padding(0, 0, 0, 10)
            };

            // Add Elite Rank selection for ships
            if (recipe.Name.Contains("(Human)") || recipe.Name.Contains("(Pirate)") || recipe.Name.Contains("(Wyrd)") 
                || recipe.Name.Contains("(Het)") || recipe.Name.Contains("(Precursor)"))
            {
                // Elite Rank Label
                Label eliteLabel = new Label
                {
                    Text = "Elite Rank:",
                    AutoSize = true,
                    Font = new Font("Segoe UI", 8F, FontStyle.Regular),
                    Margin = new Padding(0, 0, 0, 5),
                    ForeColor = Color.FromArgb(64, 64, 64)
                };
                detailsLayout.Controls.Add(eliteLabel);

                // Elite Rank ComboBox
                ComboBox eliteChoice = new ComboBox
                {
                    DropDownStyle = ComboBoxStyle.DropDownList,
                    Width = 150,
                    Margin = new Padding(0, 0, 0, 10)
                };
                eliteChoice.Items.AddRange(new string[] { "Elite 0", "Elite 1", "Elite 2", "Elite 3", "Elite 4", "Elite 5" });
                eliteChoice.SelectedIndex = 0;  // Default to Elite 0
                eliteChoice.SelectedIndexChanged += (s, e) => UpdateShipDetails(recipe, 
                    eliteChoice.SelectedItem?.ToString() ?? "Elite 0", 
                    modChoice.SelectedItem?.ToString() ?? "N/A", 
                    detailsLayout);
                detailsLayout.Controls.Add(eliteChoice);

                // Ship Mod Label
                Label modLabel = new Label
                {
                    Text = "Ship Mod:",
                    AutoSize = true,
                    Font = new Font("Segoe UI", 8F, FontStyle.Regular),
                    Margin = new Padding(0, 0, 0, 5),
                    ForeColor = Color.FromArgb(64, 64, 64)
                };
                detailsLayout.Controls.Add(modLabel);

                // Ship Mod ComboBox (already declared above)
                modChoice.Items.AddRange(resourceManager.GetAvailableShipMods(recipe.Name).ToArray());
                modChoice.SelectedIndex = 0; // Default to the first item
                modChoice.SelectedIndexChanged += (s, e) => UpdateShipDetails(recipe, 
                    eliteChoice.SelectedItem?.ToString() ?? "Elite 0", 
                    modChoice.SelectedItem?.ToString() ?? "N/A", 
                    detailsLayout);
                detailsLayout.Controls.Add(modChoice);

                // Initial display of Elite 0 and default Ship Mod details
                UpdateShipDetails(recipe, "Elite 0", modChoice.SelectedItem?.ToString() ?? "N/A", detailsLayout);
            }
            else
            {
                // Show normal recipe details for non-ship items
                var (materials, totalCost) = recipe.CalculateRequirements(1, resourceManager);
                foreach (var ingredient in materials)
                {
                    decimal resourceCost = resourceManager.GetResourcePrice(ingredient.Key) * ingredient.Value;

                    Label ingredientLabel = new Label
                    {
                        Text = $"{ingredient.Value}x {char.ToUpper(ingredient.Key[0]) + ingredient.Key.Substring(1)} ({resourceCost:N0} credits)",
                        AutoSize = true,
                        Font = new Font("Segoe UI", 8F),
                        ForeColor = Color.FromArgb(64, 64, 64)
                    };
                    detailsLayout.Controls.Add(ingredientLabel);
                }

                Label totalLabel = new Label
                {
                    Text = $"\nTotal Cost: {totalCost:N0} credits",
                    Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                    AutoSize = true,
                    Margin = new Padding(0, 10, 0, 0),
                    ForeColor = Color.FromArgb(64, 64, 64)
                };
                detailsLayout.Controls.Add(totalLabel);
            }

            recipeDetailsPanel.Controls.Add(detailsLayout);
        }

        private void UpdateShipDetails(Recipe recipe, string selectedElite, string selectedMod, FlowLayoutPanel detailsLayout)
        {
            if (selectedElite == null || selectedMod == null) return;

            // Clear existing display
            var controlsToKeep = detailsLayout.Controls.Cast<Control>()
                .Where(c => c is Label l && (l.Text == recipe.Name || l.Text == "Elite Rank:" || l.Text == "Ship Mod:") || c is ComboBox)
                .ToList();
            
            detailsLayout.Controls.Clear();
            foreach (var control in controlsToKeep)
            {
                detailsLayout.Controls.Add(control);
            }

            var (materials, baseCost) = recipe.CalculateRequirementsWithElite(1, resourceManager, selectedElite);
            decimal modAdjustment = recipe.CalculateModCostAdjustment(selectedMod, resourceManager);
            decimal totalCost = baseCost + modAdjustment;

            // Display ingredients and costs
            foreach (var material in materials)
            {
                Label ingredientLabel = new Label
                {
                    Text = $"{material.Value}x {char.ToUpper(material.Key[0]) + material.Key.Substring(1)} ({resourceManager.GetResourcePrice(material.Key) * material.Value:N0} credits)",
                    AutoSize = true,
                    Font = new Font("Segoe UI", 8F),
                    ForeColor = Color.FromArgb(64, 64, 64),
                    Tag = "requirementLabel"
                };
                detailsLayout.Controls.Add(ingredientLabel);
            }

            // Display ship mod cost if applicable
            if (selectedMod != "N/A")
            {
                Label modCostLabel = new Label
                {
                    Text = $"Ship Mod ({selectedMod}): {modAdjustment:N0} credits",
                    AutoSize = true,
                    Font = new Font("Segoe UI", 8F, FontStyle.Italic),
                    ForeColor = Color.FromArgb(64, 64, 64)
                };
                detailsLayout.Controls.Add(modCostLabel);
            }

            // Display total cost
            Label totalLabel = new Label
            {
                Text = $"\nTotal Cost: {totalCost:N0} credits",
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                AutoSize = true,
                Margin = new Padding(0, 10, 0, 0),
                ForeColor = Color.FromArgb(64, 64, 64)
            };
            detailsLayout.Controls.Add(totalLabel);
        }

        private void UpdateModFactionDetails(Recipe recipe, string selectedFaction, FlowLayoutPanel detailsLayout)
        {
            if (selectedFaction == null) return;

            // Clear existing faction requirement labels
            var controlsToRemove = detailsLayout.Controls.Cast<Control>()
                .Where(c => c.Tag?.ToString() == "factionRequirement")
                .ToList();
            foreach (var control in controlsToRemove)
            {
                detailsLayout.Controls.Remove(control);
            }

            decimal factionCost = 0;
            foreach (var req in recipe.FactionRequirements)
            {
                if (req.Key.ToLower().Contains(selectedFaction.ToLower()))
                {
                    decimal resourceCost = resourceManager.GetResourcePrice(req.Key) * req.Value;
                    factionCost += resourceCost;
                    
                    Label reqLabel = new Label
                    {
                        Text = $"{req.Value}x {char.ToUpper(req.Key[0]) + req.Key.Substring(1)} ({resourceCost:N0} credits)",
                        AutoSize = true,
                        Font = new Font("Segoe UI", 8F),
                        ForeColor = Color.FromArgb(64, 64, 64),
                        Tag = "factionRequirement"
                    };
                    detailsLayout.Controls.Add(reqLabel);
                }
            }

            // Update total cost label
            var totalLabel = detailsLayout.Controls.Cast<Control>()
                .FirstOrDefault(c => c is Label l && l.Text.StartsWith("\nTotal Cost"));
            if (totalLabel != null)
            {
                decimal baseCost = recipe.Ingredients.Sum(i => resourceManager.GetResourcePrice(i.Key) * i.Value);
                ((Label)totalLabel).Text = $"\nTotal Cost: {(baseCost + factionCost):N0} credits";
            }
        }

        private void UpdateShipEliteDetails(Recipe recipe, string selectedElite, FlowLayoutPanel detailsLayout)
        {
            if (selectedElite == null) return;

            // Clear all existing labels except the title and elite selector
            var controlsToKeep = detailsLayout.Controls.Cast<Control>()
                .Where(c => c is Label l && l.Text == recipe.Name || c is ComboBox || c is Label l2 && l2.Text == "Elite Rank:")
                .ToList();

            detailsLayout.Controls.Clear();
            foreach (var control in controlsToKeep)
            {
                detailsLayout.Controls.Add(control);
            }

            var (materials, totalCost) = recipe.CalculateRequirementsWithElite(1, resourceManager, selectedElite);

            foreach (var material in materials)
            {
                Label ingredientLabel = new Label
                {
                    Text = $"{material.Value}x {char.ToUpper(material.Key[0]) + material.Key.Substring(1)} ({resourceManager.GetResourcePrice(material.Key) * material.Value:N0} credits)",
                    AutoSize = true,
                    Font = new Font("Segoe UI", 8F),
                    ForeColor = Color.FromArgb(64, 64, 64),
                    Tag = "requirementLabel"
                };
                detailsLayout.Controls.Add(ingredientLabel);
            }

            Label totalLabel = new Label
            {
                Text = $"\nTotal Cost: {totalCost:N0} credits",
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                AutoSize = true,
                Margin = new Padding(0, 10, 0, 0),
                ForeColor = Color.FromArgb(64, 64, 64)
            };
            detailsLayout.Controls.Add(totalLabel);
        }
    }
}