using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using HCI_Project.Repos;
using HCI_Project.NotBeans;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace HCI_Project.Dijalozi
{
    /// <summary>
    /// Interaction logic for Etikete.xaml
    /// </summary>
    public partial class Etikete : Window
    {
        RepoEtikete repoEtikete = new RepoEtikete();

        Boolean ok = false;

        Window parent;

        public Boolean Ok
        {
            get { return ok; }
            set { ok = value; }
        }

        ObservableCollection<Etiketa> etikete = new ObservableCollection<Etiketa>();

        public ObservableCollection<Etiketa> Etikete1
        {
            get { return etikete; }
            set { etikete = value; }
        }
        ObservableCollection<Etiketa> etiketeLokala = new ObservableCollection<Etiketa>();

        public ObservableCollection<Etiketa> EtiketeLokala
        {
            get { return etiketeLokala; }
            set { etiketeLokala = value; }
        }

        
 
         public Etikete(Window parent)
        {
            InitializeComponent();
            this.parent = parent;
            this.etikete = repoEtikete.sveEtikete();
            this.sveEtikete.ItemsSource = this.etikete;
            initalizeEtiketeLokala();
            this.dodateEtikete.ItemsSource = this.etiketeLokala;
           
        }

         public void dodajEtiketu(object sender, EventArgs args)
         {
              Button btn = sender as Button;
              Etiketa selektovana = btn.DataContext as Etiketa;
              EtiketeLokala.Add(selektovana);
              btn.IsEnabled = false;
         }

         public void ukloniEtiketu(object sender, RoutedEventArgs args)
         {
             Button btn = sender as Button;
             Etiketa selektovana = btn.DataContext as Etiketa;
             EtiketeLokala.Remove(selektovana);
             ListBoxItem item = sveEtikete.ItemContainerGenerator.ContainerFromItem(selektovana) as ListBoxItem;
             ContentPresenter contentPresenter = FindVisualChild<ContentPresenter>(item);
             DataTemplate dataTemplate = contentPresenter.ContentTemplate;
             Button btn1 = (Button)dataTemplate.FindName("btnDodaj", contentPresenter);
             btn1.IsEnabled = true;

         }

         private childItem FindVisualChild<childItem>(DependencyObject item) where childItem: DependencyObject
         {
             for (int i = 0; i < VisualTreeHelper.GetChildrenCount(item); i++)
             {
                 DependencyObject child = VisualTreeHelper.GetChild(item, i);
                 if (child != null && child is childItem)
                     return (childItem)child;
                 else
                 {
                     childItem childOfChild = FindVisualChild<childItem>(child);
                     if (childOfChild != null)
                         return childOfChild;
                 }
             }
             return null;
         }


         private void ButtonPotvrdiClicked(object sender, RoutedEventArgs args)
         {
             ok = true;
             Lokal l = this.parent.DataContext as Lokal;
             l.Etikete = new List<Etiketa>();
             foreach (Etiketa e in this.EtiketeLokala)
             {
                 l.Etikete.Add(e);
             }
             this.parent.DataContext = l;
             this.Close();
         }

         private void ButtonOdustaniClicked(object sender, RoutedEventArgs args)
         {
             ok = false;
             this.Close();
         }

         public void initalizeEtiketeLokala()
         {
             List<Etiketa> etikete1 = (this.parent.DataContext as Lokal).Etikete;
             if (etikete1 != null)
             {
                 for (int i =0;i< etikete1.Count; i++)
                 {
                     int idx = this.sveEtikete.Items.IndexOf(etikete.ElementAt(i));
                     Etiketa e = this.sveEtikete.Items.GetItemAt(idx) as Etiketa;
                     if (e!=null && !this.etiketeLokala.Contains(e))
                         this.etiketeLokala.Add(e);

                 }
             }
         }

         public void WindowLoaded(object sender, EventArgs args)
         {
             foreach (Etiketa e in this.dodateEtikete.Items)
             {
                 ListBoxItem item = sveEtikete.ItemContainerGenerator.ContainerFromItem(e) as ListBoxItem;
                 ContentPresenter contentPresenter = FindVisualChild<ContentPresenter>(item);
                 DataTemplate dataTemplate = contentPresenter.ContentTemplate;
                 Button btn1 = (Button)dataTemplate.FindName("btnDodaj", contentPresenter);
                 btn1.IsEnabled = false;
             }
         }

    }
}
