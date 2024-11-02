using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace TheInfiniteBlack2CraftingCalculator
{
    public enum ResourceCategory
    {
        Basic,
        Relics,
        OblivionFragments,
        ShipMods  // Added new category
    }

    public class ResourceManager
    {
        private Dictionary<string, decimal> baseResourcePrices;
        private Dictionary<string, decimal> currentResourcePrices;
        private Dictionary<string, decimal> shipModPrices;
        private decimal skullPrice = 700m;

        public ResourceManager()
        {
            InitializeBaseResources();
            InitializeShipMods();
            currentResourcePrices = new Dictionary<string, decimal>(baseResourcePrices);
        }

        private void InitializeBaseResources()
        {
            baseResourcePrices = new Dictionary<string, decimal>
            {
                // Basic Resources
                { "darkmatter", 20m },
                { "radioactive", 16m },
                { "radioactives", 16m },
                { "metal", 12m },
                { "metals", 12m },
                { "gas", 6m },
                { "organic", 4m },
                { "organics", 4m },
                { "crew", 1m },
                { "skull", 700m },
                
                // Relics
                { "human relic", 5000m },
                { "het relic", 2000m },
                { "wyrd relic", 2000m },
                { "precursor relic", 10000m },

                // Oblivion Fragments
                { "human oblivion fragment", 10000m },
                { "het oblivion fragment", 10000m },
                { "wyrd oblivion fragment", 10000m },
                { "precursor oblivion fragment", 10000m }
            };
        }

        private void InitializeShipMods()
        {
            shipModPrices = new Dictionary<string, decimal>
            {
                { "N/A", 0m },
                { "Medic", 150000m },
                { "Freighter", 350000m },
                { "Raider", 20000m },
                { "Paladin", 300000m },
                { "Interdictor", 1000000m },
                { "Ultimate", 100000m },
                { "Phoenix", 25000m },
                { "Gladiator", 50000m }
            };
        }

        public decimal GetResourcePrice(string resource)
        {
            string key = resource.ToLower();
            return currentResourcePrices.ContainsKey(key) ? currentResourcePrices[key] : 0m;
        }

        public decimal GetShipModPrice(string modName)
        {
            return shipModPrices.ContainsKey(modName) ? shipModPrices[modName] : 0m;
        }

        public List<string> GetAvailableShipMods(string shipName)
        {
            var mods = new List<string> { "N/A", "Medic", "Freighter", "Raider", "Paladin", "Ultimate", "Phoenix", "Gladiator" };
            
            // Add Interdictor only for Carrier-class ships
            if (shipName.Contains("Carrier"))
            {
                mods.Add("Interdictor");
            }

            return mods;
        }

        public void SetResourcePrice(string resource, decimal price)
        {
            string key = resource.ToLower();
            if (currentResourcePrices.ContainsKey(key))
            {
                decimal minPrice = baseResourcePrices[key];
                currentResourcePrices[key] = Math.Max(price, minPrice);
            }
        }

        public void SetShipModPrice(string modName, decimal price)
        {
            if (shipModPrices.ContainsKey(modName))
            {
                shipModPrices[modName] = Math.Max(price, 0m);
            }
        }

        public decimal GetSkullPrice() => skullPrice;
        public void SetSkullPrice(decimal price) => skullPrice = Math.Max(price, baseResourcePrices["skull"]);

        public Dictionary<string, decimal> GetAllResourcePrices() => new Dictionary<string, decimal>(currentResourcePrices);
        public Dictionary<string, decimal> GetBaseResourcePrices() => new Dictionary<string, decimal>(baseResourcePrices);
        public Dictionary<string, decimal> GetShipModPrices() => new Dictionary<string, decimal>(shipModPrices);

        public Dictionary<string, decimal> GetResourcesByCategory(ResourceCategory category)
        {
            var resources = new Dictionary<string, decimal>();
            
            switch (category)
            {
                case ResourceCategory.Basic:
                    var basicResources = new[] { "darkmatter", "radioactive", "radioactives", "metal", "metals", "gas", "organic", "organics", "crew" };
                    foreach (var resource in basicResources)
                    {
                        resources.Add(resource, currentResourcePrices[resource]);
                    }
                    break;
                case ResourceCategory.Relics:
                    var relics = new[] { "human relic", "het relic", "wyrd relic", "precursor relic" };
                    foreach (var relic in relics)
                    {
                        resources.Add(relic, currentResourcePrices[relic]);
                    }
                    break;
                case ResourceCategory.OblivionFragments:
                    var fragments = new[] { "human oblivion fragment", "het oblivion fragment", "wyrd oblivion fragment", "precursor oblivion fragment" };
                    foreach (var fragment in fragments)
                    {
                        resources.Add(fragment, currentResourcePrices[fragment]);
                    }
                    break;
                case ResourceCategory.ShipMods:
                    foreach (var mod in shipModPrices)
                    {
                        resources.Add(mod.Key, mod.Value);
                    }
                    break;
            }
            
            return resources;
        }
    }

    public class ResourceConfigForm : Form
    {
        private ResourceManager resourceManager;
        private TabControl tabControl;
        private Dictionary<string, NumericUpDown> priceInputs;

        public ResourceConfigForm(ResourceManager manager)
        {
            resourceManager = manager;
            priceInputs = new Dictionary<string, NumericUpDown>();
            InitializeComponent();
            LoadResourcePrices();
            
            this.StartPosition = FormStartPosition.CenterParent;
            this.Size = new Size(300, 400);
        }

        private void InitializeComponent()
        {
            this.Text = "Resource Price Configuration";
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;

            tabControl = new TabControl
            {
                Dock = DockStyle.Fill,
                Location = new Point(0, 0)
            };

            CreateResourceTab("Basic Resources", ResourceCategory.Basic);
            CreateResourceTab("Relics", ResourceCategory.Relics);
            CreateResourceTab("Oblivion Fragments", ResourceCategory.OblivionFragments);
            CreateResourceTab("Ship Mods", ResourceCategory.ShipMods);  // Added new tab

            Button resetButton = new Button
            {
                Text = "Reset to Default",
                Dock = DockStyle.Bottom
            };
            resetButton.Click += ResetButton_Click;

            this.Controls.Add(tabControl);
            this.Controls.Add(resetButton);
        }

        private void CreateResourceTab(string title, ResourceCategory category)
        {
            TabPage tab = new TabPage(title);
            
            TableLayoutPanel layout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 2,
                AutoScroll = true,
                Padding = new Padding(10)
            };

            tab.Controls.Add(layout);
            tabControl.TabPages.Add(tab);
        }

        private void LoadResourcePrices()
        {
            LoadCategoryResources(ResourceCategory.Basic, 0);
            LoadCategoryResources(ResourceCategory.Relics, 1);
            LoadCategoryResources(ResourceCategory.OblivionFragments, 2);
            LoadCategoryResources(ResourceCategory.ShipMods, 3);  // Load ship mod prices
            AddResourceRow("skull", resourceManager.GetSkullPrice(), tabControl.TabPages[0].Controls[0] as TableLayoutPanel);
        }

        private void LoadCategoryResources(ResourceCategory category, int tabIndex)
        {
            var resources = resourceManager.GetResourcesByCategory(category);
            var layout = tabControl.TabPages[tabIndex].Controls[0] as TableLayoutPanel;
            
            foreach (var resource in resources)
            {
                AddResourceRow(resource.Key, resource.Value, layout);
            }
        }

        private void AddResourceRow(string resource, decimal price, TableLayoutPanel layout)
        {
            Label label = new Label
            {
                Text = char.ToUpper(resource[0]) + resource.Substring(1),
                AutoSize = true
            };

            NumericUpDown priceInput = new NumericUpDown
            {
                Minimum = 0,
                Maximum = 10000000,
                DecimalPlaces = 2,
                Value = price,
                Width = 100
            };

            if (resourceManager.GetBaseResourcePrices().ContainsKey(resource))
            {
                priceInput.Minimum = resourceManager.GetBaseResourcePrices()[resource];
            }

            priceInput.ValueChanged += (s, e) =>
            {
                if (resource.ToLower() == "skull")
                {
                    resourceManager.SetSkullPrice(priceInput.Value);
                }
                else if (resourceManager.GetShipModPrices().ContainsKey(resource))
                {
                    resourceManager.SetShipModPrice(resource, priceInput.Value);
                }
                else
                {
                    resourceManager.SetResourcePrice(resource, priceInput.Value);
                }
            };

            priceInputs[resource] = priceInput;

            layout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            layout.Controls.Add(label, 0, layout.RowCount - 1);
            layout.Controls.Add(priceInput, 1, layout.RowCount - 1);
        }

        private void ResetButton_Click(object sender, EventArgs e)
        {
            var baseResources = resourceManager.GetBaseResourcePrices();
            foreach (var resource in baseResources)
            {
                if (priceInputs.ContainsKey(resource.Key))
                {
                    priceInputs[resource.Key].Value = resource.Value;
                }
            }
            
            // Reset ship mod prices
            var shipMods = resourceManager.GetShipModPrices();
            foreach (var mod in shipMods)
            {
                if (priceInputs.ContainsKey(mod.Key))
                {
                    priceInputs[mod.Key].Value = mod.Value;
                }
            }
        }
    }
}